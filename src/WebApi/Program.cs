
using Contracts;
using Contracts.Filters;
using WebApi;


using RestEase.HttpClientFactory;
using WebApi.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Polly;
using Microsoft.OpenApi.Models;
using WebApi.Invoice;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(op =>
{
    op.Connect(connectionString)
        // Load all keys that start with `WebDemo:` and have no label
        // .Select("Config:*")
        .Select("*")
        // Configure to reload configuration if the registered key 'WebDemo:Sentinel' is modified.
        // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationRefreshOptions.SetCacheExpiration.
        .ConfigureRefresh(refreshOptions =>
        {
            refreshOptions.Register("Config:Virtual", refreshAll: true);
            refreshOptions.Register("ConfigInvoice", refreshAll: true);
            refreshOptions.Register("Configs", refreshAll: true);
            refreshOptions.SetCacheExpiration(TimeSpan.FromHours(2));
        })
        .UseFeatureFlags();
});

builder.Services.Configure<InvoiceSettings>(builder.Configuration.GetSection("InvoiceSettings"));
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

builder.Services.AddScoped<IRequiredHeadersFilter, SpecificHeadersFilter>();
builder.Services.AddScoped<IRequiredHeadersFilter, SpecificHeadersFilter1>();

builder.Services
    .AddRestEaseClient<ICardApi>(c =>
    {
        c.JsonSerializerSettings = customJsonSerializerSettings;
    })
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(30);
        client.BaseAddress = new Uri("https://mpfe32a1d6d88276e0b7.free.beeceptor.com");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .AddTransientHttpErrorPolicy(builder =>
        builder.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(3),
        TimeSpan.FromSeconds(7),
    }).WrapAsync(

            builder.CircuitBreakerAsync(

                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)
            )));

builder.Services.AddAzureAppConfiguration();

var configsSection = builder.Configuration.GetSection("Configs")
    .Get<IEnumerable<Configs>>()
    .ToDictionary(cw => cw.Type, cw => cw.Config);

// var invoice = builder.Configuration.GetSection("ConfigInvoice")
//     .Bind(new InvoiceSettings();


Singleton.Instance.SetValue(configsSection);
builder.Services.AddTransient<Card>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.OperationFilter<AddRequiredHeaderParameter>();
});

var app = builder.Build();
app.UseAzureAppConfiguration();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();