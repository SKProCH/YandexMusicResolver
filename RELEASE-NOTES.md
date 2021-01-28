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