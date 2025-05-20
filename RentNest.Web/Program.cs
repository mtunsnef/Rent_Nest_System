using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RentNest.Core.Configs;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Implements;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Service.Implements;
using RentNest.Service.Interfaces;

namespace RentNest.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Env.Load(); //load file .env

            builder.Services.AddDbContext<RentNestSystemContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<MailService>();
            //Service
            builder.Services.AddScoped<IAccountService, AccountService>();
            //DAO
            builder.Services.AddScoped<AccountDAO>();

            builder.Services.AddScoped<RoomDAO>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomService, RoomService>();

            builder.Services.AddScoped<PostDAO>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostService, PostService>();

            //Repository
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            //Config

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
            builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection("Authentication:Google"));
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
                options.DefaultChallengeScheme = AuthSchemes.Google;
            })
              .AddCookie(AuthSchemes.Cookie, config =>
              {
                  config.LoginPath = "/Auth/Login";
                  config.AccessDeniedPath = "/Auth/AccessDenied";
              })
              .AddGoogle(AuthSchemes.Google, options =>
              {
                  var googleAuthSettings = builder.Configuration
                      .GetSection("Authentication:Google")
                      .Get<GoogleAuthSettings>();

                  options.ClientId = googleAuthSettings.ClientId;
                  options.ClientSecret = googleAuthSettings.ClientSecret;
                  options.CallbackPath = googleAuthSettings.CallbackPath;
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
