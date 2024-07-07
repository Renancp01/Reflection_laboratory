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

    public CardService(ICardApi cardApi)
    {
        _cardApi = cardApi;
    }

    public async Task<CardResponse> GetCardAsync()
    {
        var card = await _cardApi.GetCardAsync();

        


      
        return card;
    }
}