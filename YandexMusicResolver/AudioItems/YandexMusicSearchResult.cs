using System.Collections.ObjectModel;
using Newtonsoft.Json;
using YandexMusicResolver.Loaders;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.AudioItems {
    public class YandexMusicSearchResult : IAudioItem {
        public YandexMusicSearchResult(string query, int limit, YandexSearchType type, ReadOnlyCollection<MetaAlbumSignature>? albums,
                                       ReadOnlyCollection<MetaPlaylistSignature>? playlists, ReadOnlyCollection<MetaTrack>? tracks) {
            Query = query;
            Limit = limit;
            Type = type;
            Albums = albums;
            Playlists = playlists;
            Tracks = tracks;
        }

        public string Query { get; }
        public int Limit { get; }
        public YandexSearchType Type { get; }
        public ReadOnlyCollection<MetaAlbumSignature>? Albums { get; set; }
        public ReadOnlyCollection<MetaPlaylistSignature>? Playlists { get; set; }
        public ReadOnlyCollection<MetaTrack>? Tracks { get; set; }
    }
}