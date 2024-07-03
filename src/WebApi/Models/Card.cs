using Contracts;
using Contracts.Extensions;
using WebApi.Models;

namespace WebApi.Models
{
    public class Card : Base, ICard
    {
        public Card()
        {

        }

        public Guid CardId { get; set; }

        public string Number { get; set; }

        public override List<Shortcut> Shortcuts => Singleton.Instance.Settings.Shortcuts;

        public override List<Button> Buttons => Singleton.Instance.Settings.Buttons;

        public override void AddParams()
        {
            Shortcuts.FillParamsFromSourceList(this);
            Buttons.FillParamsFromSourceList(this);
        }
    }

    public interface ICard { }
}

public class CardResponse
{
    public IEnumerable<Card> CardHolderCards { get; set; }
    public IEnumerable<Card> AdditionalCards { get; set; }
    public IEnumerable<Card> TemporaryCards { get; set; }

    public void Process()
    {
        ProcessCards(CardHolderCards);
        ProcessCards(AdditionalCards);
        ProcessCards(TemporaryCards);
    }

    private static void ProcessCards(IEnumerable<Card> cards)
    {
        if (cards == null) return;
        foreach (var card in cards)
            card.AddParams();
    }
}

public class Card1 : ICard
{
    public Guid CardId { get; set; }

    public string Number { get; set; }
}
