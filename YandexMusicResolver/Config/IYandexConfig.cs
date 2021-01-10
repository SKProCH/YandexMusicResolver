namespace YandexMusicResolver.Config {
    public interface IYandexConfig : IYandexProxyTokenHolder {
        void Load();
        void Save();
        
        string? YandexLogin { get; set; }
        string? YandexPassword { get; set; }
    }
}