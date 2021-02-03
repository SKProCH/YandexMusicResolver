# v3.0.0
- ***BREAKING CHANGES***: `LoadPlaylist` -> `LoadAlbum`, remove `almubId` from `LoadTrack`, remove track `Metadata` `IsStream`, rework `YandexMusicMainResolver` and `YandexMusicSearchResultLoader` ctors, new `YandexMusicAuth` method names, ***`Load` in config now called by loaders and can be called multiple times***
- Global type system rework: playlist now differs from album, no more meta classes as public API, new load on demand system for tracks in playlists and albums, remove `AudioTrackInfo`, now `YandexMusicTrack` are standalone, remove `IAudioItem` - `ResolveQueue` now return `YandexMusicSearchResult` and other changes
- Extend MainResolver `ResolveQuery` functionality


# v2.2.1
- Add some missing xml docs

# v2.2.0
- Add ArtworkUrl to TrackInfo and mark Metadata obsolete, mark IsStream obsolete
- Add default codec to GetDirectUrl
- Remove unnecessary albumId from track loader
- Album load method now actually called LoadAlbum
- Make meta json properties private, classes internal
- Make YandexMusicMainResolver DI suitable

# v2.1.0
- Add public members documentation

# v2.0.0
- Add new config system
- Query parser in yandex searches now separated
- Fixes for a main resolver, playlist tracks resolving, prefixes resolving
- Add Github Actions workflows
- Add unit tests

# v1.1.0
- Add authentications via YandexMusicAuth class
- New internal request structure

# v1.0.0
- Initial release