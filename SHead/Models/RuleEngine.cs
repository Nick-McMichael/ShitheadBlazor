using BlazorGames.Models.Common.Enums;

namespace SHead.Models
{
    public class RuleEngine
    {
        private static readonly Dictionary<Rank, List<Rank>> Rules;

        static RuleEngine()
        {
            Rules = new Dictionary<Rank, List<Rank>>();

            Rules.Add(Rank.Ace, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Ten, Rank.Joker });
            Rules.Add(Rank.Two, new List<Rank> { Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Three, new List<Rank> { Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Four, new List<Rank> { Rank.Ace, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Five, new List<Rank> { Rank.Two, Rank.Five, Rank.Ten });
            Rules.Add(Rank.Six, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Seven, new List<Rank> { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Joker });
            Rules.Add(Rank.Eight, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Nine, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Ten, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Jack, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.Queen, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.Ten, Rank.Queen, Rank.King, Rank.Joker });
            Rules.Add(Rank.King, new List<Rank> { Rank.Ace, Rank.Two, Rank.Five, Rank.King, Rank.Joker });
            Rules.Add(Rank.Joker, new List<Rank> { Rank.Joker });
        }

        public static bool IsCardPlayable(Card playersCard, Card tableCard)
        {
            if (tableCard == null) // no cards down, so can play anything
            {
                return true;
            }

            return Rules[tableCard.Rank].Contains(playersCard.Rank);
        }

        public static bool AreAnyCardsPlayable(List<Card> playersCards, Card tableCard)
        {
            return playersCards.Any(playersCard => IsCardPlayable(playersCard, tableCard));
        }

        public static bool AreAllCardsEqual(List<Card> cards)
        {
            return cards.All(card => card.Rank.Equals(cards[0].Rank));
        }
    }
}
