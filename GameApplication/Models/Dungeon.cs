using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameApplication.Models
{
    public class Dungeon
    {
        [Key]
        public int DungeonID { get; set; }
        public string DungeonName { get; set; }
        public string DungeonLocation { get; set; }


        //A Dungeon can have many cratures
        public ICollection<Creature> Creatures { get; set; }
    }

    public class DungeonDto
    {
        public int DungeonID { get; set; }
        public string DungeonName { get; set; }
        public string DungeonLocation { get; set; }
    }
}