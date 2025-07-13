using MonaDotNetTemplate.Repository.Interfaces;
using MonaDotNetTemplate.Repository;
using MonaDotNetTemplate.Services.Interfaces.Auth;
using MonaDotNetTemplate.Services.Services.Auth;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.OpenApi.Models;
using MonaDotNetTemplate.Services.Interfaces;
using MonaDotNetTemplate.Services.Services;
using Microsoft.Extensions.FileProviders;
using MonaDotNetTemplate.Services.Interface.Notification;
using NhapHangV2.Service.Services.Configurations;
using NhapHangV2.Service.Services.BackgroundServices;
using MonaDotNetTemplate.Services.Interfaces.BackgroundServices;
using MonaDotNetTemplate.Services.Services.BackgroundServices;
using Microsoft.Extensions.Configuration;
using MonaDotNetTemplate.Extensions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MonaDotNetTemplate.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped(typeof(IDomainRepository<>), typeof(DomainRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITokenManagerService, TokenManagerService>();
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
            services.AddScoped<IEmailConfigurationService, EmailConfigurationService>();
            services.AddHostedService<RuntimeBackgroundService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("he")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //Service DI
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountInfoService, AccountInfoService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }

        public static WebApplicationBuilder ConfigureJWT(this WebApplicationBuilder builder)
        {
            // configure strongly typed settings objects
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return builder;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MonaDotNetTeamplate API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });

                var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                foreach (var fi in dir.EnumerateFiles("*.xml"))
                {
                    c.IncludeXmlComments(fi.FullName);
                }

                c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");
                c.EnableAnnotations();
            });
            return services;
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            MonaDotNetTemplate.Extensions.HttpContext.ContextAccessor = httpContextAccessor;
            return app;
        }

        public static IApplicationBuilder UseMiddlewareContext(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }

        public static WebApplication UseStaicFilesOnServer(this WebApplication app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/Uploads"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads/Excel")),
                RequestPath = "/Excel",
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads/Image")),
                RequestPath = "/Image",
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads/PDF")),
                RequestPath = "/PDF",
            });


            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads/Word")),
                RequestPath = "/Word",
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, "Uploads/Other")),
                RequestPath = "/Other",
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            return app;
        }
    }
}
