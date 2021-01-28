using System;
using System.Collections.Generic;

namespace YandexMusicResolver {
    /// <summary>
    /// Contains info about track
    /// </summary>
    public class AudioTrackInfo {
        /// <summary>
        /// Track title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Track author
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Track lenght
        /// </summary>
        public TimeSpan Length { get; }

        /// <summary>
        /// Track identifier
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Is track live stream
        /// </summary>
        [Obsolete("There is no streams support in library. \nWill removed in 3.0")]
        public bool IsStream { get; }

        /// <summary>
        /// Track link
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Additional track metadata
        /// </summary>
        [Obsolete("Metadata used only for image uri storing. Use ArtworkUrl property instead. \nWill be removed in 3.0")]
        public Dictionary<string, string> Metadata { get; }

        /// <summary>
        /// Track image uri
        /// </summary>
        public string? ArtworkUrl { get; }

        public AudioTrackInfo(string title, string author, TimeSpan length, string identifier, bool isStream, string uri, Dictionary<string, string> metadata) {
            Title = title;
            Author = author;
            Length = length;
            Identifier = identifier;
            IsStream = isStream;
            Uri = uri;
            Metadata = metadata;
            if (Metadata.TryGetValue("artworkUrl", out var artworkUrl)) {
                ArtworkUrl = artworkUrl;
            }
        }

        public AudioTrackInfo(string title, string author, TimeSpan length, string identifier, bool isStream, string uri, string? artworkUrl = null) {
            Title = title;
            Author = author;
            Length = length;
            Identifier = identifier;
            IsStream = isStream;
            Uri = uri;
            Metadata = new Dictionary<string, string>();
            if (artworkUrl != null) {
                Metadata.Add("artworkUrl", artworkUrl);
                ArtworkUrl = artworkUrl;
            }
        }
    }
}