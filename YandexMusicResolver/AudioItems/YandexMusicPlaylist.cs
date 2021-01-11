using System.Collections.ObjectModel;

namespace YandexMusicResolver.AudioItems {
    public class YandexMusicPlaylist : IAudioItem {
        public YandexMusicPlaylist(string title, ReadOnlyCollection<YandexMusicTrack> tracks, bool isSearchResult) {
            Title = title;
            Tracks = tracks;
            IsSearchResult = isSearchResult;
        }
        public string Title { get; }
        public ReadOnlyCollection<YandexMusicTrack> Tracks { get; }
        public bool IsSearchResult { get; }
    }
}