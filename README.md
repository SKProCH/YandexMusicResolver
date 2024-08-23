# ![](https://raw.githubusercontent.com/SKProCH/YandexMusicResolver/dev/icon.png) YandexMusicResolver
A library aimed at searching, resolving and getting direct links to tracks, playlists or albums in Yandex.Music.  
Can work without authorization.

## Getting started

1. Add [nuget package](https://www.nuget.org/packages/YandexMusicResolver/) to your project:

    ```
    dotnet add package YandexMusicResolver
    ```
2. You need somehow to instantiate `YandexMusicMainResolver`.
   - If you are using dependency injection, use `.AddYandexMusicResolver()` to your `IServiceCollection`. E.g.: 
     ```csharp
     var serviceCollection = new ServiceCollection();
     serviceCollection.AddYandexMusicResolver();

     var serviceProvider = serviceCollection.BuildServiceProvider();
     // Resolve the IYandexMusicMainResolver
     var yandexMusicMainResolver = serviceProvider.GetRequiredService<IYandexMusicMainResolver>();
     ```
   - Or instantiate it as usually:
     1. Create auth service instance (`YandexMusicAuthService` this is the default implementation):
         ```csharp#
         var authService = new YandexMusicAuthService(httpClient);
         ```
         Actually, **preferred way is to use `IHttpClientFactory`** to pass it to all services.
         If you use `IHttpClientFactory` default HttpClient name is `YandexMusic`.
    
     2. Create credentials provider instance (`YandexCredentialsProvider` this is the default implementation):
         ```csharp#
         var credentialProvider = new YandexMusicAuthService(authService, "Login", "Pass");
         ```

     3. Create an instance of `YandexMusicMainResolver` and pass config to it
         ```csharp
         var yandexMusicMainResolver = new YandexMusicMainResolver(credentialProvider, httpClient);
         ```
     Full example:
     ```csharp
     var httpClient = new HttpClient();
     var authService = new YandexMusicAuthService.Create(httpClient);
     var credentialProvider = new YandexMusicAuthService.Create(authService, "Login", "Pass");
     var yandexMusicMainResolver = new YandexMusicMainResolver.Create(credentialProvider, httpClient);
     ```
3. After that you can use different methods and properties of `IYandexMusicMainResolver`.
   Example code for getting direct track download url:
   ```c#
   var directUrl = await yandexMusicMainResolver.DirectUrlLoader.GetDirectUrl("55561798");
   Console.WriteLine(directUrl);
   ```
   > [!IMPORTANT]  
   > Yandex will return a link to a 30-seconds track if you do not log in (do not use a config with a valid token).

You can take a look at unit test project for additional examples.