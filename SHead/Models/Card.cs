using BlazorGames.Models.Common.Enums;

namespace SHead.Models
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public string ImageName { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
