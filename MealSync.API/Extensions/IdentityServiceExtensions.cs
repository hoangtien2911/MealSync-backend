﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using MealSync.Application.Common.Repositories;
using MealSync.Application.Common.Services;
using MealSync.Application.Common.Services.Dapper;
using MealSync.Application.Common.Services.Notifications;
using MealSync.Application.Shared;
using MealSync.Infrastructure.Persistence.Contexts;
using MealSync.Infrastructure.Persistence.Interceptors;
using MealSync.Infrastructure.Persistence.Repositories;
using MealSync.Infrastructure.Services;
using MealSync.Infrastructure.Services.Dapper;
using MealSync.Infrastructure.Services.Notifications;
using MealSync.Infrastructure.Services.Notifications.Kafka;
using MealSync.Infrastructure.Services.Notifications.Kafka.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MealSync.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MealSync.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
    {
        // Config authentication
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["TOKEN_ISSUER"],
                ValidAudience = config["TOKEN_AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TOKEN_KEY"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };

            // Customize the response for unauthorized requests
            x.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    // Skip the default logic
                    context.HandleResponse();

                    // Set custom response
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(
                        Result.Failure(new Error("401", "Authentication failed: JWT token không hợp lệ")),
                        new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    // Set custom response
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(
                        Result.Failure(new Error("403", "Authorization failed: Bạn không có quyền truy cập")),
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    return context.Response.WriteAsync(result);
                }
            };
        });

        // Add authorization
        services.AddAuthorization();

        //Set Jwt Setting
        var jwtSetting = new JwtSetting
        {
            Key = config["TOKEN_KEY"],
            Audience = config["TOKEN_AUDIENCE"],
            Issuer = config["TOKEN_ISSUER"],
            TokenExpire = int.Parse(config["TOKEN_TIME_EXPIRED_IN_HOURS"]),
            RefreshTokenExpire = int.Parse(config["REFRESH_TOKEN_TIME_EXPIRED_IN_HOURS"])
        };

        // Set Firebase Setting
        FirebaseConfigLoader.LoadFirebaseCredentials(config["GOOGLE_APPLICATION_CREDENTIALS"]);

        // Validate the JwtSettings instance using DataAnnotations
        var validationContext = new ValidationContext(jwtSetting);
        Validator.ValidateObject(jwtSetting, validationContext, validateAllProperties: true);

        // Register the JwtSettings instance as a singleton
        services.AddSingleton<JwtSetting>(jwtSetting);

        // Set Redis Setting
        var redidSetting = new RedisSetting()
        {
            ConnectionString = config["REDIS_URL"]
        };

        // Validate the RedisSettings instance using DataAnnotations
        var validationRedisContext = new ValidationContext(redidSetting);
        Validator.ValidateObject(redidSetting, validationRedisContext, validateAllProperties: true);

        // Register the RedisSettings instance as a singleton
        services.AddSingleton<RedisSetting>(redidSetting);

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redidSetting.ConnectionString));
        services.AddStackExchangeRedisCache(option => option.Configuration = redidSetting.ConnectionString);

        // Set VnPay Setting
        var vnPaySetting = new VnPaySetting
        {
            PaymentUrl = config["VN_PAY_PAYMENT_URL"] ?? string.Empty,
            ReturnUrl = config["VN_PAY_RETURN_URL"] ?? string.Empty,
            RefundUrl = config["VN_PAY_REFUND_URL"] ?? string.Empty,
            TmpCode = config["VN_PAY_TMP_CODE"] ?? string.Empty,
            HashSecret = config["VN_PAY_HASH_SECRET"] ?? string.Empty,
        };

        // Validate the VnPaySettings instance using DataAnnotations
        var validationVnPayContext = new ValidationContext(vnPaySetting);
        Validator.ValidateObject(vnPaySetting, validationVnPayContext, validateAllProperties: true);

        // Register the RedisSettings instance as a singleton
        services.AddSingleton<VnPaySetting>(vnPaySetting);

        //Config service
        var assembly = typeof(BaseService).Assembly;
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(BaseService))
            .AddClasses(classes => classes.AssignableTo(typeof(IBaseService)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .FromAssembliesOf(typeof(AccountRepository)) // Infrastructure Layer
            .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        //Add unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDapperService, DapperService>();
        services.AddScoped<CustomSaveChangesInterceptor>();
        services.AddDbContext<MealSyncContext>(options =>
        {
            options.UseMySql(config["DATABASE_URL"], ServerVersion.Parse("8.0.33-mysql"))
                .UseSnakeCaseNamingConvention();
        });

        // Add notification service
        services.AddSingleton<IMobileNotificationService, FirebaseNotificationService>();
        services.AddSingleton<IWebNotificationService, KafkaPushNotificationService>();
        services.AddSingleton<INotificationProvider, NotificationProvider>();
        services.AddSingleton<INotifierService, NotifierService>();
        services.AddSingleton<INotificationFactory, NotificationFactory>();

        // Kafka
        services.AddHostedService<KafkaConsumerRequestNotiFirebaseService>();
        services.AddHttpClient();

        // Add Error Config
        var resourceRepository = services.BuildServiceProvider().GetService<ISystemResourceRepository>();
        Error.Configure(resourceRepository);

        return services;
    }
}