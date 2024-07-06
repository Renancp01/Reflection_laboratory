
using Contracts;
using WebApi;


using RestEase.HttpClientFactory;
using WebApi.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(op =>
{
    op.Connect(connectionString)
        // Load all keys that start with `WebDemo:` and have no label
        .Select("Config:*")
        // Configure to reload configuration if the registered key 'WebDemo:Sentinel' is modified.
        // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationRefreshOptions.SetCacheExpiration.
        .ConfigureRefresh(refreshOptions =>
        {
            refreshOptions.Register("Config:Virtual", refreshAll: true);
            refreshOptions.Register("Configs", refreshAll: true);
            refreshOptions.SetCacheExpiration(TimeSpan.FromHours(2));
        })
        .UseFeatureFlags();
});

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Config:Virtual"));
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));

builder.Services.AddTransient<CardService>();
builder.Services.AddAzureAppConfiguration();

var customJsonSerializerSettings = new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver(),
    NullValueHandling = NullValueHandling.Ignore,
    Formatting = Formatting.Indented
};

customJsonSerializerSettings.Converters.Add(new CardConverter());

builder.Services
    //.AddTransient(_ => new CustomResponseHandler())
    //.AddRestEaseClient<ICardApi>("https://cards.free.beeceptor.com")
    .AddRestEaseClient<ICardApi>(c =>
    {
        c.JsonSerializerSettings = customJsonSerializerSettings;
    })
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://cards.free.beeceptor.com");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });
//.AddHttpMessageHandler<CustomResponseHandler>();

builder.Services.AddAzureAppConfiguration();
var configsSection = builder.Configuration.GetSection("Configs")
    .Get<IEnumerable<Configs>>()
    .ToDictionary(cw => cw.Type, cw => cw.Config);

Singleton.Instance.SetValue(configsSection);
builder.Services.AddTransient<Card>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAzureAppConfiguration();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();