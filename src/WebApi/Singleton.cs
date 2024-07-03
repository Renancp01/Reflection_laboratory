using Contracts;

namespace WebApi;

public class Singleton
{
    private static readonly Lazy<Singleton> instance = new(() => new Singleton());

    private Singleton() { }

    public static Singleton Instance => instance.Value;

    public Settings Settings{ get; set; }

    public void SetValue(Settings settings)
    {
        Settings = settings;
    }

    public Settings GetValue()
    {
        return Settings;
    }
}