using DotNetEnv;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Configs;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Service.Implements;
using RentNest.Service.Interfaces;

namespace RentNest.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            //load file .env
            var webRoot = builder.Environment.ContentRootPath;   
            var solutionRoot = Path.GetFullPath(Path.Combine(webRoot, ".."));
            var envPath = Path.Combine(solutionRoot, ".env");

            Env.Load(envPath);

            builder.Services.AddDbContext<RentNestSystemContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<MailService>();

            //Service
            builder.Services.AddScoped<IAccountService, AccountService>();

            //DAO
            builder.Services.AddScoped<AccountDAO>(); //????

            //Config
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            //su dung cache de luu session
            builder.Services.AddDistributedMemoryCache();

            //add session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthSchemes.Cookie;
                options.DefaultSignInScheme = AuthSchemes.Cookie;
            })
                .AddCookie(AuthSchemes.Cookie, config =>
                {
                    config.LoginPath = "/Auth/Login";
                    config.AccessDeniedPath = "/Auth/AccessDenied";
                })
                .AddGoogle(AuthSchemes.Google, options =>
                {
                    options.ClientId = AuthSettings.GoogleClientId;
                    options.ClientSecret = AuthSettings.GoogleClientSecret;
                    options.CallbackPath = "/Auth/signIn-google";
                })
                .AddFacebook(AuthSchemes.Facebook, options =>
                {
                    options.AppId = AuthSettings.FacebookAppId;
                    options.AppSecret = AuthSettings.FacebookAppSecret;
                    options.CallbackPath = "/Auth/signIn-facebook";
                    options.Events = new OAuthEvents
                    {
                        OnRemoteFailure = context =>
                        {
                            context.Response.Redirect("/Auth/Login");
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });


            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/Home");
                    return;
                }
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
