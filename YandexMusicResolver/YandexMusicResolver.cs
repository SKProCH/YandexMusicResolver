﻿using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver {
    public class YandexMusicResolver {
        private const string TrackUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)/track/([0-9]+)$";
        private const string AlbumUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)$";
        private const string PlaylistUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/users/(.+)/playlists/([0-9]+)$";

        private static Regex TrackUrlRegex = new Regex(TrackUrlPattern);
        private static Regex AlbumUrlRegex = new Regex(AlbumUrlPattern);
        private static Regex PlaylistUrlRegex = new Regex(PlaylistUrlPattern);

        public virtual YandexMusicPlaylistLoader PlaylistLoader { get; }
        public virtual YandexMusicTrackLoader TrackLoader { get; }
        public virtual YandexMusicDirectUrlLoader DirectUrlLoader { get; }
        public virtual YandexMusicSearchResultLoader SearchResultLoader { get; }

        public YandexMusicResolver(bool allowSearch = true,
                                   YandexMusicPlaylistLoader? playlistLoader = null,
                                   YandexMusicTrackLoader? trackLoader = null,
                                   YandexMusicDirectUrlLoader? directUrlLoader = null,
                                   YandexMusicSearchResultLoader? searchResultLoader = null) {
            PlaylistLoader = playlistLoader ?? new YandexMusicPlaylistLoader();
            TrackLoader = trackLoader ?? new YandexMusicTrackLoader();
            DirectUrlLoader = directUrlLoader ?? new YandexMusicDirectUrlLoader();
            SearchResultLoader = searchResultLoader ?? new YandexMusicSearchResultLoader();
            AllowSearch = allowSearch;
        }
        
        public bool AllowSearch { get; }

        public async Task<IAudioItem?> ResolveQuery(string query, bool? allowSearchOverride = null) {
            var trackMatch = TrackUrlRegex.Match(query);
            if (trackMatch.Success) {
                return await TrackLoader.LoadTrack(trackMatch.Groups[1].Value, trackMatch.Groups[2].Value, GetTrack);
            }

            var playlistMatch = PlaylistUrlRegex.Match(query);
            if (playlistMatch.Success) {
                return await PlaylistLoader.LoadPlaylist(trackMatch.Groups[1].Value, trackMatch.Groups[2].Value, GetTrack);
            }

            var albumMatch = AlbumUrlRegex.Match(query);
            if (albumMatch.Success) {
                return await PlaylistLoader.LoadPlaylist(trackMatch.Groups[1].Value, "volumes", GetTrack);
            }
            
            if (allowSearchOverride ?? AllowSearch) {
                return await SearchResultLoader.LoadSearchResult(query, PlaylistLoader, GetTrack);
            }
            return null;
        }

        private YandexMusicTrack GetTrack(AudioTrackInfo arg) {
            return new YandexMusicTrack(arg, this);
        }
    }
}