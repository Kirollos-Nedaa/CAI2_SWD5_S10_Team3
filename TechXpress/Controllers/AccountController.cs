using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net;
using System.Security.Claims;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Domain.ViewModels;

namespace TechXpress.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CartServices _cartService;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager , CartServices cartService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _emailSender = emailSender;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate the verification code before creating the user
                var verificationCode = new Random().Next(100000, 999999).ToString();

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    VerificationCode = verificationCode
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Send the verification code via SendGrid
                    await _emailSender.SendDynamicEmailAsync(
                        user.Email,
                        "Verify your account",
                        "d-8f54da15ccfb4583851af2129f8a8991", // your template ID
                        new { verification_code = verificationCode });

                    return RedirectToAction("VerifyCode", new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult VerifyCode(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var model = new VerifyCodeViewModel { UserId = userId };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            if (user.VerificationCode == model.Code)
            {
                user.EmailConfirmed = true;
                user.VerificationCode = null;
                await _userManager.UpdateAsync(user);
                return View("ConfirmEmailSuccess", user.Id);
            }

            ModelState.AddModelError("Code", "Invalid verification code.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendVerificationCode(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (user.EmailConfirmed)
                return RedirectToAction("Login");

            // Generate and update a new code
            var newCode = new Random().Next(100000, 999999).ToString();
            user.VerificationCode = newCode;
            await _userManager.UpdateAsync(user);

            await _emailSender.SendDynamicEmailAsync(
                user.Email,
                "Verify your account",
                "d-8f54da15ccfb4583851af2129f8a8991",
                new { verification_code = newCode });

            TempData["Message"] = "A new verification code has been sent to your email.";
            return RedirectToAction("VerifyCode", new { userId });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AutoLoginAfterConfirm(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user.EmailConfirmed)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            // Step 1: Validate user existence
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password are incorrect.");
                return View(model);
            }

            // Step 2: Check password correctness
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Email or password are incorrect.");
                return View(model);
            }

            // Step 3: Email not confirmed → resend code and show toast
            if (!user.EmailConfirmed)
            {
                // Generate and store a new verification code
                var newCode = new Random().Next(100000, 999999).ToString();
                user.VerificationCode = newCode;
                await _userManager.UpdateAsync(user);

                // Send new code via email
                await _emailSender.SendDynamicEmailAsync(
                    user.Email,
                    "Verify your account",
                    "d-8f54da15ccfb4583851af2129f8a8991",
                    new { verification_code = newCode });

                // Store toast message + email temporarily for the view
                TempData["ResendVerificationToast"] = $"Please verify your email to continue. <a href='/Account/VerifyCode?userId={user.Id}' class='underline'>Click here</a> to enter your code.";
                TempData["UnverifiedUserEmail"] = user.Email;

                return RedirectToAction("Login");
            }

            // Step 4: All good → sign in user
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                await _cartService.MergeCartsAsync(user.Id);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Email or password are incorrect.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            // Force Google to prompt account selection
            properties.Items["prompt"] = "select_account";

            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

            // Try to sign in using external login
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // Check if user exists by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Auto-confirm if not already confirmed (optional)
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }

                // Link external login to existing user
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            // Create new user with confirmed email
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // If creation failed
            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var model = new AccountViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditAccount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditAccountViewModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Profile updated successfully!";
                return RedirectToAction("MyAccount");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                // Re-sign in the user to update authentication cookie
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("MyAccount");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmEmailSuccess()
        {
            return View();
        }
    }
}
