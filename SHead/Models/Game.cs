using BlazorGames.Models.Common.Enums;
using SHead.Models.Enums;
using System;

namespace SHead.Models
{
    public class Game
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(Game));

        public int Id { get; set; }
        public List<Player> Players { get; internal set; }
        public CardDeck Deck { get; internal set; }
        public List<Card> PickUpPack { get; internal set; }
        public List<Card> ClearedCards { get; internal set; }
        public bool CanJoin { get; private set; }
        public bool HasStarted { get; private set; }

        public Player CurrentPlayer
        {
            get
            {
                return Players.SingleOrDefault(o => o.IsThisPlayersTurn);
            }
        }

        internal Direction Direction { get; set; }

        public Game()
        {
            Deck = new CardDeck();
            PickUpPack = new List<Card>();
            ClearedCards = new List<Card>();
            Direction = Direction.Forwards;
            Players = new List<Player>();
            CanJoin = true;
        }

        public void AddPlayer(Player player)
        {
            //player.CurrentGame = this;
            Players.Add(player);
        }

        public void StartNewGame()
        {
            Players[0].IsThisPlayersTurn = true;
            Players[0].IsAbleToPlay = true;
            Deck.Shuffle();
            DealStartingCards();
            CanJoin = false;
            HasStarted = true;
            //Log.Info("New Game Started.");
        }

        public void PlayCards(Guid playerId, List<Card> cards)
        {
            if (cards.Count == 0)
            {
                CantPlay();
                return;
            }

            if (CheckFor4OfTheSameRank(cards))
            {
                PlayTenOr4OfSameRank(cards);
                return;
            }

            try
            {
                ValidatePlayedCards(playerId, cards);
            }
            catch (InvalidCardException)
            {
                if (CurrentPlayer.PlayerState.Equals(PlayerState.PlayingFaceDownCards))
                {
                    foreach (Card card in cards)
                    {
                        RemoveCardFromHand(card);
                        CurrentPlayer.hand.InHandCards.Add(card);
                    }
                    CantPlay();
                    return;
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }

            PlayRank(cards);
        }

        private void CantPlay()
        {
            PickEmUp();
            CheckNextPlayerCanPlay();
            SetNextPlayerActive();
        }

        private void PlayRank(List<Card> cards)
        {
            switch (cards[0].Rank)
            {
                case Rank.Five:
                    PlayFive(cards);
                    SetNextPlayerActive();
                    SetNextPlayerActive();
                    break;
                case Rank.Ten:
                    PlayTenOr4OfSameRank(cards);
                    if (CurrentPlayer.PlayerState.Equals(PlayerState.HasNoCards))
                    {
                        SetNextPlayerActive();
                    }
                    break;
                case Rank.Queen:
                    PlayEight(cards);
                    CheckNextPlayerCanPlay();
                    SetNextPlayerActive();
                    break;
                default:
                    PlaySameOrHigher(cards);
                    CheckNextPlayerCanPlay();
                    SetNextPlayerActive();
                    break;
            }
        }

        private bool CheckFor4OfTheSameRank(List<Card> cards)
        {
            Rank rank = cards[0].Rank;
            int numOfPickupPackCardsToCheck = 4 - cards.Count;

            if (cards.All(o => o.Rank == rank))
            {
                if (PickUpPack.Count == 0 || numOfPickupPackCardsToCheck > PickUpPack.Count) return false;

                for (int i = PickUpPack.Count - 1; i > PickUpPack.Count - numOfPickupPackCardsToCheck - 1; i--)
                {
                    if (PickUpPack[i].Rank != rank)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void PickEmUp()
        {
            CurrentPlayer.hand.InHandCards.AddRange(PickUpPack);
            PickUpPack.Clear();
        }

        private void CheckNextPlayerCanPlay()
        {
            var nextPlayer = GetNextPlayer();
            List<Card> inPlayCards = GetActiveHandPart(nextPlayer);
            Card topOfPickUpPack = PickUpPack != null && PickUpPack.Count > 0 ? PickUpPack.Last() : null;

            nextPlayer.IsAbleToPlay = nextPlayer.PlayerState.Equals(PlayerState.PlayingFaceDownCards) || RuleEngine.AreAnyCardsPlayable(inPlayCards, topOfPickUpPack);
        }

        private List<Card> GetActiveHandPart(Player player)
        {
            if (player.PlayerState.Equals(PlayerState.PlayingHandCards))
            {
                return player.hand.InHandCards;
            }

            if (player.PlayerState.Equals(PlayerState.PlayingFaceUpCards))
            {
                return player.hand.FaceUpCards;
            }

            if (player.PlayerState.Equals(PlayerState.PlayingFaceDownCards))
            {
                return player.hand.FaceDownCards;
            }

            return null;
        }

        private void PlayEight(IEnumerable<Card> cards)
        {
            if (cards.Count() == 1 || cards.Count() == 3)
            {
                ChangeDirection();
            }

            PlaySameOrHigher(cards);
        }

        private void ChangeDirection()
        {
            Direction = Direction == Direction.Forwards ? Direction.Backwards : Direction.Forwards;
        }

        private void PlayTenOr4OfSameRank(IEnumerable<Card> cards)
        {
            ClearedCards.AddRange(PickUpPack);
            PickUpPack.Clear();
            foreach (Card card in cards)
            {
                RemoveCardFromHand(card);
                ClearedCards.Add(card);
            }
        }

        private void PlayFive(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                RemoveCardFromHand(card);
                ClearedCards.Add(card);
            }

            var nextPlayer = GetNextPlayer();
            nextPlayer.hand.InHandCards.AddRange(PickUpPack);
            PickUpPack.Clear();
        }

        private void PlaySameOrHigher(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                RemoveCardFromHand(card);
                PickUpPack.Add(card);
            }
        }

        private void RemoveCardFromHand(Card card)
        {
            Card matchingInHandCard = CurrentPlayer.hand.InHandCards.SingleOrDefault(o => o.Rank == card.Rank && o.Suit == card.Suit);

            if (matchingInHandCard != null)
            {
                CurrentPlayer.hand.InHandCards.Remove(matchingInHandCard);
                return;
            }

            Card matchingFaceUpCard = CurrentPlayer.hand.FaceUpCards.SingleOrDefault(o => o.Rank == card.Rank && o.Suit == card.Suit);

            if (matchingFaceUpCard != null)
            {
                CurrentPlayer.hand.FaceUpCards.Remove(matchingFaceUpCard);
                return;
            }

            Card matchingFaceDownCard = CurrentPlayer.hand.FaceDownCards.SingleOrDefault(o => o.Rank == card.Rank && o.Suit == card.Suit);

            if (matchingFaceDownCard != null)
            {
                CurrentPlayer.hand.FaceDownCards.Remove(matchingFaceDownCard);
            }
        }

        private void ValidatePlayedCards(Guid playerId, List<Card> cards)
        {
            Player player = Players.Single(o => o.Id.Equals(playerId));

            if (!player.IsThisPlayersTurn)
            {
                throw new InvalidOperationException("It's not this player's turn. ");
            }

            if (cards.Count > 1 && !RuleEngine.AreAllCardsEqual(cards))
            {
                throw new InvalidOperationException("Can only play cards of the same rank. ");
            }

            Card topOfPickUpPack = PickUpPack != null && PickUpPack.Count > 0 ? PickUpPack.Last() : null;

            if (!RuleEngine.IsCardPlayable(cards[0], topOfPickUpPack))
            {
                throw new InvalidCardException("Can't play a " + cards[0].Rank);
            }
        }

        private void SetNextPlayerActive()
        {
            var nextPlayer = GetNextPlayer();
            CurrentPlayer.IsThisPlayersTurn = false;
            nextPlayer.IsThisPlayersTurn = true;
        }

        public Player GetNextPlayer()
        {
            int currentPlayerIndex = Players.FindIndex(o => o.IsThisPlayersTurn);
            int next = Direction.Equals(Direction.Forwards) ? 1 : -1;

            int nextPlayerIndex = currentPlayerIndex;
            do
            {
                nextPlayerIndex = (nextPlayerIndex + next + Players.Count) % Players.Count;
            } while (Players[nextPlayerIndex].PlayerState == PlayerState.HasNoCards && nextPlayerIndex != currentPlayerIndex);

            return Players[nextPlayerIndex];
        }

        private void DealStartingCards()
        {
            DealCards(hand => hand.FaceDownCards);
            DealCards(hand => hand.FaceUpCards);
            DealCards(hand => hand.InHandCards);
        }

        private void DealCards(Func<Hand, List<Card>> handProjection)
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (var player in Players)
                {
                    List<Card> cards = handProjection(player.hand);
                    if (cards.Count < 3)
                    {
                        if (Deck.Count > 0)
                        {
                            cards.Add(Deck.Draw());
                        }
                    }
                }
            }
        }
    }
}
