using System;
using Newtonsoft.Json;

#pragma warning disable 8618

namespace YandexMusicResolver.Responces {
    internal class MetaAccount {
        [JsonProperty("now")]
        public DateTimeOffset Now { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("region")]
        public long Region { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("secondName")]
        public string SecondName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("serviceAvailable")]
        public bool ServiceAvailable { get; set; }

        [JsonProperty("registeredAt")]
        public DateTimeOffset RegisteredAt { get; set; }
    }
}