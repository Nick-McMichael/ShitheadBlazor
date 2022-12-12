namespace SHead.Models
{
    public class PileBase
    {
        protected List<Card> Cards { get; set; } = new List<Card>();

        public void Add(Card card)
        {
            if (card != null)
                Cards.Add(card);
        }

        public bool Any()
        {
            return Cards.Any();
        }

        public Card Pop()
        {
            var lastCard = Cards.LastOrDefault();
            if (lastCard != null)
            {
                Cards.Remove(lastCard);
            }

            return lastCard;
        }

        public Card Last()
        {
            return Cards.LastOrDefault();
        }
    }
}
