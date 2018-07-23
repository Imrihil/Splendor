using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splendor.Models
{
    public class Aristocrat
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int RequireGreen { get; set; }
        public int RequireWhite { get; set; }
        public int RequireBlue { get; set; }
        public int RequireBlack { get; set; }
        public int RequireRed { get; set; }
        [Display(Name="Image")]
        public string ImagePath { get; set; }
    }
}