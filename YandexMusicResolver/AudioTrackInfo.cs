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
        /// Track link
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Track image uri
        /// </summary>
        public string? ArtworkUrl { get; }

        public AudioTrackInfo(string title, string author, TimeSpan length, string identifier, string uri, string? artworkUrl = null) {
            Title = title;
            Author = author;
            Length = length;
            Identifier = identifier;
            Uri = uri;
            ArtworkUrl = artworkUrl;
        }
    }
}