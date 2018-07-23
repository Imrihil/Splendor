using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Points { get; set; }
        [Display(Name = "Gem")]
        public int GemId { get; set; }
        [ForeignKey("GemId")]
        public virtual Gem Gem { get; set; }
        public int Lvl { get; set; }
        public int CostGreen { get; set; }
        public int CostWhite { get; set; }
        public int CostBlue { get; set; }
        public int CostBlack { get; set; }
        public int CostRed { get; set; }
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
        public string BackPath
        {
            get
            {
                switch (Lvl)
                {
                    case 3: return "~/Images/Back/back3.png";
                    case 2: return "~/Images/Back/back2.png";
                    default: return "~/Images/Back/back1.png";
                }
            }
        }
    }
}
