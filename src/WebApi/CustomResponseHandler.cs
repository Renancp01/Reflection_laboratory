using Newtonsoft.Json;
using System.Text;
using WebApi.Models;

namespace WebApi;

public class CustomResponseHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var cards = JsonConvert.DeserializeObject<CardResponse>(content);
            
            cards.Process<CardResponse, Card, Config>(card => Singleton.Instance.GetValue(card.Type));

            var modifiedContent = new StringContent(JsonConvert.SerializeObject(cards), Encoding.UTF8, "application/json");

            response.Content = modifiedContent;
        }

        return response;
    }

    private void Process(IEnumerable<Card> cards)
    {
        foreach (var card in cards)
        {
            var config = Singleton.Instance.GetValue(card.Type);

            card.Buttons = config.Buttons;
            card.Shortcuts = config.Shortcuts;
        }
    }
}