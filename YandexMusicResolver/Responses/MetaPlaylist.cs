using System.Collections.Generic;
using System.Linq;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    internal class MetaPlaylist : MetaPlaylistSignature {
        public List<MetaPlaylistTrackContainer>? Tracks { get; set; }

        public override YandexMusicPlaylist ToYaPlaylist(IYandexMusicPlaylistLoader yandexMusicPlaylistLoader) {
            var tracks = Tracks!
                .Select(container => container.Track?.ToYmTrack())
                .OfType<YandexMusicTrack>()
                .ToList();
            return new YandexMusicPlaylist(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), tracks);
        }

        public YandexMusicPlaylist ToYaPlaylist(IEnumerable<YandexMusicTrack> fallBackTracks) {
            var fallbackTracksDictionary = fallBackTracks.ToDictionary(track => track.Id);
            var tracks = Tracks!
                .Select(container => container.Track?.ToYmTrack() ?? fallbackTracksDictionary[container.Id])
                .ToList();
            return new YandexMusicPlaylist(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), tracks);
        }
    }
}
