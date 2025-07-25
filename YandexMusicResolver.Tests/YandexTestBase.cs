﻿using System.IO;
using System.Net.Http;
using Moq;
using Moq.AutoMock;
using YandexMusicResolver.Config;

namespace YandexMusicResolver.Tests;

public class YandexTestBase {
    public AutoMocker AutoMocker = new();
    public Mock<IYandexCredentialsProvider> YandexCredentialsProviderMock;
    public YandexMusicMainResolver MainResolver;

    public YandexTestBase() {
        YandexCredentialsProviderMock = AutoMocker.GetMock<IYandexCredentialsProvider>();
        MainResolver = YandexMusicMainResolver.Create(YandexCredentialsProviderMock.Object, new HttpClient());
        MainResolver.PlainTextIsSearchQuery = false;
    }
}