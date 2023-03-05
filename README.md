# ![](https://raw.githubusercontent.com/SKProCH/YandexMusicResolver/dev/icon.png) YandexMusicResolver
A library aimed at searching, resolving and getting direct links to tracks, playlists or albums in Yandex.Music.  
Can work without authorization.

## Getting started

1. Add [nuget package](https://www.nuget.org/packages/YandexMusicResolver/) to your project:

    ```
    dotnet add package YandexMusicResolver
    ```
2. Create auth service instance (`YandexMusicAuthService` this is the default implementation):
    ```c#
    var authService = new YandexMusicAuthService(httpClient);
    ```
    Actually, **preferred way is to use `IHttpClientFactory`** to pass it to all services.
    If you use `IHttpClientFactory` default HttpClient name is `YandexMusic`.
    
3. Create credentials provider instance (`YandexCredentialsProvider` this is the default implementation):
    ```c#
    var credentialProvider = new YandexMusicAuthService(authService, "Login", "Pass");
    ```

4. Create an instance of `YandexMusicMainResolver` and pass config to it
    ```c#
    var yandexMusicMainResolver = new YandexMusicMainResolver(credentialProvider, httpClient);
    ```
    After that we can use `YandexMusicMainResolver` methods and other loaders methods.

Example code for getting direct track download url:
```c#
var httpClient = new HttpClient();
var authService = new YandexMusicAuthService(httpClient);
var credentialProvider = new YandexMusicAuthService(authService, "Login", "Pass");
var yandexMusicMainResolver = new YandexMusicMainResolver(credentialProvider, httpClient);
var directUrl = await yandexMusicMainResolver.DirectUrlLoader.GetDirectUrl("55561798");
Console.WriteLine(directUrl);
```
**Warn:** Yandex will return a link to a 30-seconds track if you do not log in (do not use a config with a valid token).

You can take a look at unit test project for additional examples.