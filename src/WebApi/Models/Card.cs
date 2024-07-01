using Contracts;
using Contracts.Extensions;

namespace WebApi.Models
{
    public class Card : Base
    {
        public Guid CardId { get; set; }

        public string Number { get; set; }

        public override List<Shortcut> Shortcuts { get; set; }

        public override List<Button> Buttons { get; set; }

        public override void AddParams()
        {
            Shortcuts.FillParamsFromSourceList(this);
            Buttons.FillParamsFromSourceList(this);
        }
    }
}
