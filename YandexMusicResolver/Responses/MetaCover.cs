namespace YandexMusicResolver.Responses {
    internal class MetaCover {
        public string Type { get; set; } = null!;

        public string? Dir { get; set; }

        public string? Version { get; set; }

        public string? Uri { get; set; }

        public bool Custom { get; set; }

        public string? GetCoverUrl() {
            if (Uri == null) return null;
            return "https://" + Uri.Replace("%%", "200x200");
        }
    }
}
