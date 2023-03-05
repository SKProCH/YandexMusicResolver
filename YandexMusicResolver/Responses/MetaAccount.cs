using System;
#pragma warning disable 8618

namespace YandexMusicResolver.Responses {
    internal class MetaAccount {
        public DateTimeOffset Now { get; set; }

        public string Uid { get; set; }

        public string Login { get; set; }

        public long Region { get; set; }

        public string FullName { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string DisplayName { get; set; }

        public bool ServiceAvailable { get; set; }

        public DateTimeOffset RegisteredAt { get; set; }
    }
}
