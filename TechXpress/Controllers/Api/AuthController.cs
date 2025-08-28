using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;
using TechXpress.Infrastructure.Contexts;

namespace TechXpress.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly AppDbContext _context;
        private readonly CartServices _cartService;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IGoogleTokenValidator googleTokenValidator,
            AppDbContext context,
            CartServices cartService,
            ILogger<AuthController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _googleTokenValidator = googleTokenValidator;
            _context = context;
            _cartService = cartService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "InvalidRequest", message = "Invalid login request" });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { error = "InvalidCredentials", message = "Invalid email or password" });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized(new { error = "InvalidCredentials", message = "Invalid email or password" });

            if (!user.EmailConfirmed)
                return Unauthorized(new { error = "UnconfirmedEmail", message = "Please confirm your email." });

            // Merge cart (matching web logic)
            await _cartService.MergeCartsAsync(user.Id);

            // Generate JWT and Refresh Token
            var jwtToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token in database
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                ReplacedByToken = null // Explicitly set to null for new tokens
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = jwtToken,
                refreshToken = refreshToken,
            });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "InvalidRequest", message = "Invalid Google login request" });

            var payload = await _googleTokenValidator.ValidateAsync(model.IdToken);
            if (payload == null)
                return BadRequest(new { error = "InvalidToken", message = "Invalid Google ID token" });

            var email = payload.Email;
            var name = payload.Name ?? email;

            // Check if user exists by email
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // Auto-confirm email if not already confirmed (matching web logic)
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }

                // Check if Google login already exists
                var existingLogins = await _userManager.GetLoginsAsync(user);
                if (!existingLogins.Any(l => l.LoginProvider == "Google" && l.ProviderKey == payload.Subject))
                {
                    // Link external login to existing user
                    await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
                }
            }
            else
            {
                // Create new user with confirmed email (matching web logic)
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return BadRequest(new { error = "UserCreationFailed", message = "Failed to create user account" });
                }

                // Add external login
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
            }

            // Merge cart (matching web logic)
            await _cartService.MergeCartsAsync(user.Id);

            // Generate JWT and Refresh Token
            var jwtToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Store refresh token in database
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(30)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = jwtToken,
                refreshToken = refreshToken,
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    dateOfBirth = user.DateOfBirth,
                    gender = user.Gender
                }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel model)
        {
            if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest(new { error = "InvalidRequest", message = "Token and refresh token are required" });
            }

            try
            {
                var principal = _jwtService.GetPrincipalFromExpiredToken(model.Token);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { error = "InvalidToken", message = "Invalid token" });
                }

                // Find the refresh token in database
                var refreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken &&
                                            rt.UserId == userId &&
                                            !rt.IsRevoked &&
                                            rt.ExpiryDate > DateTime.UtcNow);

                if (refreshToken == null)
                {
                    return Unauthorized(new { error = "InvalidRefreshToken", message = "Invalid or expired refresh token" });
                }

                // Get the user
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { error = "UserNotFound", message = "User not found" });
                }

                // Generate new tokens
                var newJwtToken = _jwtService.GenerateJwtToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                // Revoke old refresh token
                refreshToken.IsRevoked = true;
                refreshToken.ReplacedByToken = newRefreshToken;

                // Add new refresh token
                _context.RefreshTokens.Add(new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = userId,
                    ExpiryDate = DateTime.UtcNow.AddDays(30)
                });

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { error = "TokenValidationFailed", message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the full exception
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new { error = "ServerError", message = ex.Message, stack = ex.StackTrace });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // generate verification code
            var code = new Random().Next(100000, 999999).ToString();
            user.VerificationCode = code;
            await _userManager.UpdateAsync(user);

            // send code via email (SendGrid)
            await _emailSender.SendEmailAsync(user.Email, "Verify your email",
                $"Your verification code is: {code}");

            return Ok(new { userId = user.Id, message = "User registered. Verification code sent to email." });
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return BadRequest("User not found");

            if (user.EmailConfirmed)
                return BadRequest("Email already confirmed");

            user.EmailConfirmed = true;
            user.VerificationCode = null;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Email confirmed successfully" });
        }

        [HttpPost("resend-code")]
        public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return BadRequest("User not found");

            var code = new Random().Next(100000, 999999).ToString();
            user.VerificationCode = code;
            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(user.Email, "Resend verification code",
                $"Your new verification code is: {code}");

            return Ok(new { message = "Verification code resent" });
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { error = "InvalidToken", message = "Invalid or missing token" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { error = "UserNotFound", message = "User not found" });

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new { error = "PasswordChangeFailed", message = "Failed to change password", errors });
            }

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutViewModel model)
        {
            if (!string.IsNullOrEmpty(model.RefreshToken))
            {
                var refreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken);

                if (refreshToken != null)
                {
                    refreshToken.IsRevoked = true;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
