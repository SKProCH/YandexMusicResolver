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
        public bool IsStream { get; }

        /// <summary>
        /// Track link
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Additional track metadata
        /// </summary>
        public Dictionary<string, string> Metadata { get; }

        public AudioTrackInfo(string title, string author, TimeSpan length, string identifier, bool isStream, string uri, Dictionary<string, string> metadata) {
            Title = title;
            Author = author;
            Length = length;
            Identifier = identifier;
            IsStream = isStream;
            Uri = uri;
            Metadata = metadata;
        }
    }
}