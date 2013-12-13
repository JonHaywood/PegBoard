using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PegBoard.Web.Models
{
    public class Score
    {
        [Key]
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public int PegCount { get; set; }
        public double TotalTimeInSeconds { get; set; }
    }
}