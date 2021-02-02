﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// AudioTrackInfo wrapper to resolve track direct url
    /// </summary>
    public class YandexMusicTrack  {
        internal YandexMusicTrack(string title, List<YandexMusicArtist> authors, TimeSpan length, string id, string uri, string? artworkUrl = null) {
            Title = title;
            Authors = authors;
            Length = length;
            Id = id;
            Uri = uri;
            ArtworkUrl = artworkUrl;
        }
        
        /// <summary>
        /// Track title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Track authors
        /// </summary>
        public List<YandexMusicArtist> Authors { get; }

        /// <summary>
        /// Compose <see cref="Authors"/> names into single string
        /// </summary>
        public string Author => string.Join(", ", Authors.Select(artist => artist.Name));

        /// <summary>
        /// Track lenght
        /// </summary>
        public TimeSpan Length { get; }

        /// <summary>
        /// Track id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Track link
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Track image uri
        /// </summary>
        public string? ArtworkUrl { get; }
    }
}