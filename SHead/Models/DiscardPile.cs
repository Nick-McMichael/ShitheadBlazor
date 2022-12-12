namespace SHead.Models
{
    public class DiscardPile : PileBase
    {
        public List<Card> GetAll()
        {
            return Cards.ToList();
        }
    }
}
