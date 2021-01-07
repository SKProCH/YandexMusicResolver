using System;
using System.Collections.Generic;

namespace YandexMusicResolver {
    public class AudioTrackInfo {
        public string Title { get; }
        public string Author { get; }
        public long Length { get; }
        public string Identifier { get; }
        public bool IsStream { get; }
        public string Uri { get; }
        public Dictionary<string, string> Metadata { get; }

        public AudioTrackInfo(string title, string author, long length, string identifier, bool isStream, string uri, Dictionary<string, string> metadata) {
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