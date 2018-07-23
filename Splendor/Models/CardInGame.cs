using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.Models
{
    public class CardInGame
    {
        public int Id { get; set; }
        [Display(Name = "Card")]
        public int CardId { get; set; }
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
        [Display(Name = "PlayerInGame")]
        public int PlayerInGameId { get; set; }
        [ForeignKey("PlayerInGameId")]
        public virtual PlayerInGame PlayerInGame { get; set; }
        public bool IsOnTable { get; set; }
    }
}
