namespace SHead.Models
{
    public class Hand
    {
        public List<Card> FaceDownCards { get; internal set; }
        public List<Card> FaceUpCards { get; internal set; }
        public List<Card> InHandCards { get; internal set; }

        public Hand()
        {
            FaceDownCards = new List<Card>(3);
            FaceUpCards = new List<Card>(3);
            InHandCards = new List<Card>();
        }

        public Hand(List<Card> faceDownCards, List<Card> faceUpCards, List<Card> inHandCards)
        {
            FaceDownCards = faceDownCards;
            FaceUpCards = faceUpCards;
            InHandCards = inHandCards;
        }
    }
}
