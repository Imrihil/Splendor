using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Splendor.Models
{
    public class Log
    {
        public static string System = "system";
        public static string GameLog = "game";
        public static string Warning = "warning";
        public static string Error = "error";
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        [Display(Name = "Game")]
        public int? GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }
        public string Message { get; set; }
        public Log()
        {
            Time = DateTime.Now;
        }
        public Log(string userName, string type, int? gameId, string message)
        {
            Time = DateTime.Now;
            UserName = userName;
            Type = type;
            GameId = gameId;
            Message = message;
        }
    }
}