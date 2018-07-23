using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.Models
{
    public class AristocratInGame
    {
        public int Id { get; set; }
        [Display(Name = "Aristocrat")]
        public int AristocratId { get; set; }
        [ForeignKey("AristocratId")]
        public virtual Aristocrat Aristocrat { get; set; }
        [Display(Name = "PlayerInGame")]
        public int PlayerInGameId { get; set; }
        [ForeignKey("PlayerInGameId")]
        public virtual PlayerInGame PlayerInGame { get; set; }
    }
}
