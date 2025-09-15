// -----------------------------------------------------------------------------
// RhSenso Web - Program.cs
// Interface MVC com autenticação por cookies e integração com API
// -----------------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RhS.Infrastructure.Data;
using RhS.SharedKernel.Extensions;
using RhS.SharedKernel.Interfaces;
using RhS.Web.Services.Security;
using RhS.Web.Services.ApiClients;

namespace RhS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------------------------------------------
            // Services
            // ---------------------------------------------------------------
            builder.Services.AddControllersWithViews();
            builder.Services.ConfigureHttpJsonOptions(o =>
            {
                o.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                o.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            // Entity Framework
            builder.Services.AddDbContext<RhSensoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                                   builder.Configuration.GetConnectionString("Default")));

            // HttpContextAccessor e TenantAccessor
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ITenantAccessor, TenantAccessor>();

            // SharedKernel
            builder.Services.AddSharedKernel();

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(60);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".RhSenso.Auth";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            builder.Services.AddAuthorization();

            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".RhSenso.Antiforgery";
                options.HeaderName = "RequestVerificationToken";
            });

            // Permission Provider
            builder.Services.AddScoped<IPermissionProvider, PermissionProvider>();

            // Logging
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.SetMinimumLevel(LogLevel.Information);
                builder.Logging.AddFilter("RhS.Web.Services.Security", LogLevel.Information);
                builder.Logging.AddFilter("RhS.Web.TagHelpers", LogLevel.Information);
                builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Information);
            }

            // API Configuration
            var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7000/";
            if (!apiBaseUrl.EndsWith("/")) apiBaseUrl += "/";

            // HTTP Client for API
            builder.Services.AddHttpClient("RhSensoApi", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // API Services
            builder.Services.AddScoped<IBotoesApi, BotoesApi>();
            builder.Services.AddScoped<BotoesGateway>();

            var app = builder.Build();

            // ---------------------------------------------------------------
            // Middleware Pipeline
            // ---------------------------------------------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            // Routes
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

