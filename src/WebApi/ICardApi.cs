using RestEase;

namespace WebApi;

public interface ICardApi
{
    [Get("/api/v1/activeFunction")]
    [AllowAnyStatusCode]
    Task<Response<CardResponse>> GetCardAsync();
}