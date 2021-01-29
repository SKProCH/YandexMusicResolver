﻿using System;
using System.IO;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;

namespace YandexMusicResolver.Tests {
    public class YandexTestBase {
        public IYandexConfig Config;
        public YandexMusicMainResolver MainResolver;
        public Func<AudioTrackInfo, YandexMusicTrack> TrackFactory;

        public YandexTestBase() {
            if (File.Exists("TestData.json")) {
                Config = new FileYandexConfig("TestData.json");
            }
            else {
                Config = new EnvironmentConfig();
            }
            
            MainResolver = new YandexMusicMainResolver(Config);
            TrackFactory = info => new YandexMusicTrack(info, MainResolver);
        }
    }
}