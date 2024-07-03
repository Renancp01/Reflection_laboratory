using System.Diagnostics;
using Contracts;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Models;

namespace WebApi;

public class CardService
{
    private readonly ICardApi _cardApi;
    private readonly IOptions<Settings> _settings;

    public CardService(IConfiguration configuration, ICardApi cardApi)
    {
        _cardApi = cardApi;
    }

    public async Task<CardResponse> GetCardAsync()
    {
        var stop1 = new Stopwatch();
        var card = await _cardApi.GetCardAsync();

        stop1.Start();
        card.Process();
        stop1.Stop();
        Console.WriteLine($"tempo: {stop1.ElapsedMilliseconds} ms");
        return card;
    }
}