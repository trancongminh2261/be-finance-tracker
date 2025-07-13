using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MonaDotNetTemplate.API;
using MonaDotNetTemplate.API.Extensions;
using MonaDotNetTemplate.Entities.Base;
using MonaDotNetTemplate.Entities.Entities;
using MonaDotNetTemplate.Extensions;
using MonaDotNetTemplate.Utilities;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

internal class Program
{
    readonly static string SignalROrigins = "SignalROrigins";
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContextPool<AppDbContext>(optionsAction: options => options.UseSqlServer(builder.Configuration.GetConnectionString("MonaDotNetTemplateContext")));
        // Add services to the container.
        builder.Services.ConfigureRepositoryWrapper();
        builder.Services.ConfigureService();
        builder.Services.ConfigureSwagger();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDistributedMemoryCache();

        builder.ConfigureJWT();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
            foreach(var cor in builder.Configuration.GetSection("Cors").GetChildren())
            {
                options.AddPolicy(cor["Name"],
                builder =>
                {
                    builder
                    .WithOrigins(cor["Domain"])
                    .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   ;
                });
            }
            options.AddPolicy(SignalROrigins,
            builder =>
            {
                builder
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .SetIsOriginAllowed(hostName => true);
            });
        });

        builder.Services.AddSignalR().AddJsonProtocol(options =>
        {
            //Viết hoa chữ cái đầu
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        });

        var app = builder.Build();

        app.UseCookiePolicy();

        app.UseRouting();

        foreach (var cor in builder.Configuration.GetSection("Cors").GetChildren())
        {
            app.UseCors(cor["Name"]);
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseStaicFilesOnServer();

        app.UseStaticHttpContext();

        app.UseMiddlewareContext();

        //app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseEndpoints(endpoints => endpoints.MapHub<DomainHub>("/hubs").RequireCors(SignalROrigins));

        app.Run();
    }
}