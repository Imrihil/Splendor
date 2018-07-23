using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.Models
{
    public class PlayerInGame
    {
        public int Id { get; set; }
        public int ResourceGreen { get; set; }
        public int ResourceWhite { get; set; }
        public int ResourceBlue { get; set; }
        public int ResourceBlack { get; set; }
        public int ResourceRed { get; set; }
        public int ResourceGold { get; set; }
        public int TakeGreen { get; set; }
        public int TakeWhite { get; set; }
        public int TakeBlue { get; set; }
        public int TakeBlack { get; set; }
        public int TakeRed { get; set; }
        public bool IsWiner { get; set; }
        [Display(Name = "Game")]
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }
        [Display(Name = "Player")]
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }
        [Display(Name = "Aristocrats")]
        public bool IsActive { get; set; }
        public bool IsReserving { get; set; }
        public int NextPlayerInGameId { get; set; }
        public virtual ICollection<AristocratInGame> AristocratInGames { get; set; }
        [Display(Name = "Cards")]
        public virtual ICollection<CardInGame> CardInGames { get; set; }
        public int Points
        {
            get
            {
                int result = 0;
                foreach(var cardInGame in CardInGames.Where(x => x.IsOnTable))
                {
                    result += cardInGame.Card.Points;
                }
                foreach (var AristocratInGame in AristocratInGames)
                {
                    result += AristocratInGame.Aristocrat.Points;
                }
                return result;
            }
        }
        public string Name { get { return Player.UserName; } }
        public bool IsCollectingGems { get { return TakenGems > 0; } }
        public int TakenGems { get { return TakeGreen + TakeWhite + TakeBlue + TakeBlack + TakeRed; } }
        public int AllResources { get { return ResourceGreen + ResourceWhite + ResourceBlue + ResourceBlack + ResourceRed + ResourceGold; } }

        public bool CanCollect(int gemId)
        {
            if (AllResources + TakenGems >= 10 || TakeGreen == 2 || TakeWhite == 2 || TakeBlue == 2 || TakeBlack == 2 || TakeRed == 2) return false;
            PlayerInGame tableInGame = Game.TableInGame;
            switch (gemId)
            {
                case Gem.green:
                    return (TakeGreen == 0 && tableInGame.ResourceGreen > 0 && TakenGems < 3) || (TakeGreen == 1 && tableInGame.ResourceGreen > 3 && TakenGems < 2);
                case Gem.white:
                    return (TakeWhite == 0 && tableInGame.ResourceWhite > 0 && TakenGems < 3) || (TakeWhite == 1 && tableInGame.ResourceWhite > 3 && TakenGems < 2);
                case Gem.blue:
                    return (TakeBlue == 0 && tableInGame.ResourceBlue > 0 && TakenGems < 3) || (TakeBlue == 1 && tableInGame.ResourceBlue > 3 && TakenGems < 2);
                case Gem.black:
                    return (TakeBlack == 0 && tableInGame.ResourceBlack > 0 && TakenGems < 3) || (TakeBlack == 1 && tableInGame.ResourceBlack > 3 && TakenGems < 2);
                case Gem.red:
                    return (TakeRed == 0 && tableInGame.ResourceRed > 0 && TakenGems < 3) || (TakeRed == 1 && tableInGame.ResourceRed > 3 && TakenGems < 2);
                default:
                    return false;
            }
        }
        public bool CanAfford(CardInGame cardInGame)
        {
            int costGold;
            return CanAfford(cardInGame, out costGold);
        }
        public bool CanAfford(CardInGame cardInGame, out int costGold)
        {
            costGold = 0;
            costGold += Math.Max(cardInGame.Card.CostGreen - CardInGames.Where(x => x.Card.GemId == Gem.green && x.IsOnTable == true).Count() - ResourceGreen, 0);
            costGold += Math.Max(cardInGame.Card.CostWhite - CardInGames.Where(x => x.Card.GemId == Gem.white && x.IsOnTable == true).Count() - ResourceWhite, 0);
            costGold += Math.Max(cardInGame.Card.CostBlack - CardInGames.Where(x => x.Card.GemId == Gem.black && x.IsOnTable == true).Count() - ResourceBlack, 0);
            costGold += Math.Max(cardInGame.Card.CostBlue - CardInGames.Where(x => x.Card.GemId == Gem.blue && x.IsOnTable == true).Count() - ResourceBlue, 0);
            costGold += Math.Max(cardInGame.Card.CostRed - CardInGames.Where(x => x.Card.GemId == Gem.red && x.IsOnTable == true).Count() - ResourceRed, 0);
            return ResourceGold >= costGold;
        }
        public int GetResource(int gemId)
        {
            switch (gemId)
            {
                case Gem.green:
                    return ResourceGreen;
                case Gem.white:
                    return ResourceWhite;
                case Gem.blue:
                    return ResourceBlue;
                case Gem.black:
                    return ResourceBlack;
                case Gem.red:
                    return ResourceRed;
                case Gem.gold:
                    return ResourceGold;
                default:
                    throw new ArgumentException("There are only 6 kind of gems in the game");
            }
        }
        public int GetTakeGem(int gemId)
        {
            switch (gemId)
            {
                case Gem.green:
                    return TakeGreen;
                case Gem.white:
                    return TakeWhite;
                case Gem.blue:
                    return TakeBlue;
                case Gem.black:
                    return TakeBlack;
                case Gem.red:
                    return TakeRed;
                default:
                    throw new ArgumentException("There are only 5 kind of gems to collect in the game");
            }
        }
    }
}
