using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(40, ErrorMessage = "Maximal length of the name of a game is 40 characters!")]
        public string Name { get; set; }
        public int FirstPlayerInGameId { get; set; }
        [Display(Name = "Players")]
        public virtual ICollection<PlayerInGame> PlayerInGames { get; set; }
        public Game()
        {
            CreationTime = DateTime.Now;
        }
        public bool IsStarted { get { return FirstPlayerInGameId == 0 ? false : true; } }
        public PlayerInGame TableInGame { get { return PlayerInGames.FirstOrDefault(x => x.PlayerId.Equals(1)); } }
        public bool IsEnded { get { return WinnerPlayer != null; } }
        public PlayerInGame WinnerPlayer { get { if (PlayerInGames == null) return null; return PlayerInGames.FirstOrDefault(x => x.IsWiner); } }
    }
}
