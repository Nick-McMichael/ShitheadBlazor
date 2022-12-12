using SHead.Models.Enums;

namespace SHead.Models
{
    public class Player
    {
        public Guid Id { get; internal set; }
        //public Game CurrentGame { get; internal set; }
        public Hand hand { get; internal set; }
        public bool IsThisPlayersTurn { get; internal set; }
        public bool IsAbleToPlay { get; internal set; }
        public string Name { get; internal set; }

        public PlayerState PlayerState
        {
            get
            {
                if (this.hand.InHandCards.Count > 0)
                {
                    return PlayerState.PlayingHandCards;
                }

                if (this.hand.FaceUpCards.Count > 0)
                {
                    return PlayerState.PlayingFaceUpCards;
                }

                if (this.hand.FaceDownCards.Count > 0)
                {
                    return PlayerState.PlayingFaceDownCards;
                }

                return PlayerState.HasNoCards;
            }
        }

        public Player(string name) : this()
        {
            Name = name;
        }

        public Player(Hand hand, string name)
        {
            Name = name;
            IsAbleToPlay = true;
            hand = hand;
            Id = Guid.NewGuid();
        }

        public Player()
        {
            IsAbleToPlay = true;
            hand = new Hand();
            Id = Guid.NewGuid();
        }
    }
}
