using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Contracts;
using Contracts.Extensions;
using WebApi.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using WebApi;
using RestEase;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using RestEase.HttpClientFactory;

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
            refreshOptions.SetCacheExpiration(TimeSpan.FromHours(2));
        })
        .UseFeatureFlags();
});

builder.Services.Configure<Settings>(builder.Configuration.GetSection("Config:Virtual"));
builder.Services.AddTransient<CardService>();
builder.Services.AddAzureAppConfiguration();

builder.Services
    .AddRestEaseClient<ICardApi>("https://cards.free.beeceptor.com");

builder.Services.AddAzureAppConfiguration();

var a = builder.Configuration.GetSection("Config:Virtual").Get<Settings>();

Singleton.Instance.SetValue(a);
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