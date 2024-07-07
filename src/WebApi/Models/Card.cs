using Contracts;
using WebApi.Models;

namespace WebApi.Models
{
    public class Card : Base
    {
        public string Type { get; set; }

        public Guid CardId { get; set; }

        public string Number { get; set; }
        public override List<Shortcut> Shortcuts { get; set; }
        public override List<Button> Buttons { get; set; }
        public override void AddParams()
        {
            throw new NotImplementedException();
        }
    }
}

public class CardResponse
{
    public IEnumerable<Card> CardHolderCards { get; set; }
    
    public IEnumerable<Card> AdditionalCards { get; set; }
    
    public IEnumerable<Card> TemporaryCards { get; set; }
}