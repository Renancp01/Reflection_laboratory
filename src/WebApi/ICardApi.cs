using RestEase;

namespace WebApi;

public interface ICardApi
{
    [Get("/api/v2/cards?includeDebit=true")]
    Task<Response<CardResponse>> GetCardAsync();
}