using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechXpress.Core.Services;
using TechXpress.Domain.Models;
using TechXpress.Infrastructure;
using TechXpress.Infrastructure.Contexts;
using TechXpress.Domain.Profiles;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// Configure Services
// =============================================

// Add MVC with Views
builder.Services.AddControllersWithViews();

// Configure Session
builder.Services.AddDistributedMemoryCache(); // Consider Redis for production
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure Application Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

// Application Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Profiles));

// Business Logic Services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BrandServices>();
builder.Services.AddScoped<CartServices>();
builder.Services.AddScoped<WishlistService>();

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// =============================================
// Configure Application Pipeline
// =============================================

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Security Middleware
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Seed Initial Data
await SeedDatabaseAsync(app.Services);

// Configure Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.Run();

// =============================================
// Database Seeding
// =============================================
async Task SeedDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Seed Roles
        var roles = new[] { "Admin", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@techxpress.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin User",
                Gender="Male",
                DateOfBirth = new DateOnly(1990, 1, 1),
                EmailConfirmed = true,
                PhoneNumber = "+1234567890"
            };

            var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}