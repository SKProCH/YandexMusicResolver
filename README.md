<table align="center"><tr><td align="center" width="9999">
<img src="https://download.cdn.yandex.net/from/yandex.ru/support/ru/music/files/icon_main.png" align="center" alt="Project icon" height="150">

  <h1 style="margin:-30px auto auto auto">YandexMusicResolver</h1>

A library aimed at searching, resolving and getting direct links to tracks, playlists or albums in Yandex.Music. Can work without authorization.
</td></tr>
</table>

## Getting started

<ol type="1">

<li>

Add [nuget package](https://www.nuget.org/packages/YandexMusicResolver/) to your project:

```
dotnet add package YandexMusicResolver
```
</li>

<li>

Create configuration instance (`FileYandexConfig` this is the default implementation to save the config to a file)  :

```c#
var config = new FileYandexConfig("path to store config");
```
or use empty config (if you don't want save anything):
```c#
var config = new EmptyYandexConfig();
```
After that call `Load` to load config:
```c#
config.Load();
```
</li>

<li>

Create an instance of `YandexMusicMainResolver` and pass config to it
```c#
var yandexMusicMainResolver = new YandexMusicMainResolver(config);
```
After that we can use `YandexMusicMainResolver` methods and other loaders methods.
</li>

</ol>

Example code for getting direct track download url:
```c#
var fileYandexConfig = new FileYandexConfig("yandex.config");
fileYandexConfig.Load();
var yandexMusicMainResolver = new YandexMusicMainResolver(fileYandexConfig);
var directUrl = await yandexMusicMainResolver.DirectUrlLoader.GetDirectUrl("55561798");
Console.WriteLine(directUrl);
```
**Warn:** Yandex will return a link to a 30-seconds track if you do not log in (do not use a config with a valid token).

Methods to assist with authorization can be found in `YandexMusicAuth`.

For additional examples you can take a look at unit test project.