using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Contracts;
using Contracts.Extensions;

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
        //.ConfigureRefresh(refreshOptions =>
        //{
        //    refreshOptions.Register("WebDemo:Sentinel", refreshAll: true);
        //})
        // Load all feature flags with no label. To load specific feature flags and labels, set via FeatureFlagOptions.Select.
        // Use the default cache expiration of 30 seconds. It can be overriden via FeatureFlagOptions.CacheExpirationInterval.
        .UseFeatureFlags();
});

//builder.Services.Configure<Settings>(options =>
//{
//    var rawJson = builder.Configuration["RawJsonConfig"];
    
//    JsonSerializer.Deserialize(rawJson, options);
//});

builder.Services.AddContracts(builder.Configuration);

builder.Services.AddAzureAppConfiguration();
    //.AddFeatureManagement();
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