using Microsoft.Extensions.Options;

namespace Api;

public static class StartupExtensions
{
    public static TSettings ConfigureSettings<TSettings>(this IServiceCollection services,
        IConfiguration configuration,
        string configurationSectionKey) where TSettings : class, new()
    {
        var settings = configuration.GetSection(configurationSectionKey);

        services.Configure<TSettings>(settings);
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TSettings>>().Value);
        return settings.Get<TSettings>()!;
    }
}
