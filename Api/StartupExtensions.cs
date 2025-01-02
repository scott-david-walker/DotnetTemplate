using System.Reflection;
using Api.Behaviours;
using Api.Framework;
using Api.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class StartupExtensions
{
    private static TSettings ConfigureSettings<TSettings>(this IServiceCollection services,
        IConfiguration configuration,
        string configurationSectionKey) where TSettings : class, new()
    {
        var settings = configuration.GetSection(configurationSectionKey);

        services.Configure<TSettings>(settings);
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TSettings>>().Value);
        return settings.Get<TSettings>()!;
    }

    public static IServiceCollection ConfigureMediatrBehaviours(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.Scan(scan => scan
            .FromAssemblyOf<IAuthoriser>()
            .AddClasses(classes => classes.AssignableTo<IAuthoriser>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorisationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection ConfigureAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings =
            services.ConfigureSettings<JwtSettings>(configuration, nameof(JwtSettings));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.GetKey())
                };
            });
        return services;
    }
}
