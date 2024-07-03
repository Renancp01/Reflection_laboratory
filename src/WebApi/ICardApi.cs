using RestEase;
using WebApi.Models;

namespace WebApi;

public interface ICardApi
{
    [Get("/api/v2/cards?includeDebit=true")]
    Task<CardResponse> GetCardAsync();
}