using Contracts;

namespace WebApi;

public class Singleton
{
    private static readonly Lazy<Singleton> instance = new(() => new Singleton());

    private Singleton() { }

    public static Singleton Instance => instance.Value;

    public Dictionary<string, Config> Settings { get; set; }

    public void SetValue(Dictionary<string, Config> settings)
    {
        Settings = settings;
    }

    public Config GetValue(string key)
    {
        return Settings.GetValueOrDefault(key);
    }
}