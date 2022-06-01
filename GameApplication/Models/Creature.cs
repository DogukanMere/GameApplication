using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameApplication.Models
{
    public class Creature
    {
        [Key]
        public int CreatureID { get; set; }
        public string CreatureName { get; set; }
        public int CreaturePower { get; set; }

        //Todo - Race
        //A Creature belongs to one race
        //Many Creatures can have same race
        [ForeignKey("Race")]
        public int RaceID { get; set; }
        public virtual Race Race { get; set; }


        //A Creature can be in many dungeons
        public ICollection<Dungeon> Dungeons { get; set; }
    }

    public class CreatureDto
    {
        public int CreatureID { get; set; }
        public string CreatureName { get; set; }
        public int CreaturePower { get; set; }
        public int RaceID { get; set; }
        public string RaceName { get; set; }
    }

}