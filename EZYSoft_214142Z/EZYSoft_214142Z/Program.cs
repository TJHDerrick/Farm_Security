using AspNetCore.ReCaptcha;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EZYSoft_214142Z.Model;
using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleReCaptcha"));


//.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);


//builder.Services.Configure<IdentityOptions>(
//    opts => opts.SignIn.RequireConfirmedEmail = true);


//builder.Services.AddDbContext<AuthDbContext>();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>()
//         .AddDefaultTokenProviders(); ;
builder.Services.AddRazorPages();
//builder.Services.AddDataProtection();



builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Tokens.AuthenticatorTokenProvider = "Email";
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    //options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

//services
builder.Services.AddSingleton<IUserTwoFactorTokenProvider<ApplicationUser>, EmailTokenProvider<ApplicationUser>>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();


// Email config
var configuration = builder.Configuration;
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.AccessDeniedPath = "/Login";
});

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.ExpireTimeSpan = TimeSpan.FromSeconds(45);
    Config.SlidingExpiration = true;
    Config.Cookie.Name = "MyCookieAuth";
    Config.LoginPath = "/Login";
    Config.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
    options.ValidationInterval = TimeSpan.FromSeconds(2)
);

//builder.Services.AddAuthentication()
//    .AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = configuration["App:GoogleClientId"];
//    googleOptions.ClientSecret = configuration["App:GoogleClientSecret"];
//});

// Facebook login
builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = configuration["FacebookAppId"];
    facebookOptions.AppSecret = configuration["FacebookAppSecret"];
});

//builder.Services.AddScoped<ApplicationUser>();

//controller stuff
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// session stuff
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseMiddleware<SecurityStampValidatorMiddleware>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.Run();