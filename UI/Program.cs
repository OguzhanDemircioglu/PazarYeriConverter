using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UI.Middleware;
using UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<AuthorizationService>(_ => new AuthorizationService(null));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
    options.Cookie.Name = "Pazaryeri";
    options.Cookie.MaxAge = TimeSpan.FromHours(30);

});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services
    .AddCookiePolicy(options =>
    {

    })
    .AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Configure your token validation parameters
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7083",
            ValidAudience = "https://localhost:7083",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "ca11764e72f26f2f9486b5c8d5ce69e73ca11764e72f26f2f9486b5c8d5ce69e738f5c3ba45b0296a2b61bab2e926eb85472653dd4b482a7955641e499cfc6a89e7a73dff145fb88992fbc3fceba96ad78f5c3ba45b029ca11764e72f26f2f9486b5c8d5ce69e73ca11764e72f26f2f9486b5c8d5ce69e738f5c3ba45b0296a2b61bab2e926eb85472653dd4b482a7955641e499cfc6a89e7a73dff145fb88992fbc3fceba96ad78f5c3ba45b0296a2b61bab2e926eb85472653dd4b482a7955641e499cfc6a89e7a73dff145fb88992fbc3fceba96ad76a2b61bab2e926eb85472653dd4b482a7955641e499cfc6a89e7a73dff145fb88992fbc3fceba96ad7"))
        };
    });



var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{

    if (context.Request.Path.HasValue)
    {
        if (context.Request.Path.Value == "/")
        {
            context.Response.Redirect("/auth/login");
            return;
        }

    }
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});


app.UseMiddleware<CustomAuthorizationMiddleware>();




app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
