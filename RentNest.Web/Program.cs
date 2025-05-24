using DotNetEnv;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.SignalR;
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
using RentNest.Web.Hubs;

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

            //Add dbcontext
            builder.Services.AddDbContext<RentNestSystemContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //DAO
            builder.Services.AddScoped<AccountDAO>();
            builder.Services.AddScoped<UserProfileDAO>();
            builder.Services.AddScoped<AccommodationTypeDAO>();
            builder.Services.AddScoped<UserProfileDAO>();
            builder.Services.AddScoped<AmenitiesDAO>();
            builder.Services.AddScoped<TimeUnitPackageDAO>();
            builder.Services.AddScoped<PackagePricingDAO>();
            builder.Services.AddScoped<AccommodationDAO>();
            builder.Services.AddScoped<PostDAO>();
            builder.Services.AddScoped<ConversationDAO>();
            builder.Services.AddScoped<MessageDAO>();

            //Repository
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccommodationTypeRepository, AccommodationTypeRepository>();
            builder.Services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
            builder.Services.AddScoped<ITimeUnitPackageRepository, TimeUnitPackageRepository>();
            builder.Services.AddScoped<IPackagePricingRepository, PackagePricingRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            //Service
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAzureOpenAIService, AzureOpenAIService>();
            builder.Services.AddScoped<IAccommodationTypeService, AccommodationTypeService>();
            builder.Services.AddScoped<IAmenitiesSerivce, AmenitiesService>();
            builder.Services.AddScoped<ITimeUnitPackageService, TimeUnitPackageService>();
            builder.Services.AddScoped<IPackagePricingService, PackagePricingService>();
            builder.Services.AddScoped<IAccommodationService, AccommodationService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            builder.Services.AddScoped<IConversationService, ConversationService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            //Config
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<AzureOpenAISettings>(options =>
            {
                options.Endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
                options.DeploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT")!;
                options.ApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")!;
            });
            builder.Services.Configure<AuthSettings>(options =>
            {
                options.Google.ClientId = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENTID")!;
                options.Google.ClientSecret = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_CLIENTSECRET")!;
                options.Facebook.AppId = Environment.GetEnvironmentVariable("AUTHENTICATION_FACEBOOK_APPID")!;
                options.Facebook.AppSecret = Environment.GetEnvironmentVariable("AUTHENTICATION_FACEBOOK_APPSECRET")!;
            });
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            builder.Services.AddSignalR()
                .AddHubOptions<ChatHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                });

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

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
            var authSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<AuthSettings>>().Value;

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
                    options.ClientId = authSettings.Google.ClientId;
                    options.ClientSecret = authSettings.Google.ClientSecret;
                    options.CallbackPath = "/Auth/signIn-google";
                })
                .AddFacebook(AuthSchemes.Facebook, options =>
                {
                    options.AppId = authSettings.Facebook.AppId;
                    options.AppSecret = authSettings.Facebook.AppSecret;
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

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/trang-chu/Error");
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
                    context.Response.Redirect("/trang-chu");
                    return;
                }
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chathub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
