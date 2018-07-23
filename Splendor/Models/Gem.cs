using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splendor.Models
{
    public class Gem
    {
        public const int green = 1;
        public const int white = 2;
        public const int blue = 3;
        public const int black = 4;
        public const int red = 5;
        public const int gold = 6;

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(40, ErrorMessage = "Maximal length of the name of a game is 40 characters!")]
        public string Name { get; set; }
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
    }
}