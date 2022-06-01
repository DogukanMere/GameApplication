using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameApplication.Models
{
    public class Race
    {
        [Key]
        public int RaceID { get; set; }
        public string RaceName { get; set; }
        public bool RaceOffensive { get; set; }
    }

    public class RaceDto
    {
        public int RaceID { get; set; }
        public string RaceName { get; set; }
        public bool RaceOffensive { get; set; }
    }
}