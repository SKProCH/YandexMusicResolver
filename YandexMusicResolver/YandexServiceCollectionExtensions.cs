using Microsoft.Extensions.DependencyInjection;
using YandexMusicResolver.Config;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver;

public static class YandexServiceCollectionExtensions {
    public static IServiceCollection AddYandexMusicResolver(this IServiceCollection services) {
        services.AddOptions<YandexCredentials>();

        services.AddHttpClient();
        services.AddSingleton<IYandexCredentialsProvider, YandexCredentialsProvider>();
        services.AddSingleton<IYandexMusicAuthService, YandexMusicAuthService>();
        services.AddSingleton<IYandexMusicDirectUrlLoader, YandexMusicDirectUrlLoader>();
        services.AddSingleton<IYandexMusicPlaylistLoader, YandexMusicPlaylistLoader>();
        services.AddSingleton<IYandexMusicSearchResultLoader, YandexMusicSearchResultLoader>();
        services.AddSingleton<IYandexMusicTrackLoader, YandexMusicTrackLoader>();
        services.AddSingleton<IYandexMusicMainResolver, YandexMusicMainResolver>();

        return services;
    }
}
