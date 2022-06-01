using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameApplication.Models.ViewModels
{
    public class UpdateCreature
    {
        //Existing Creature info
        public CreatureDto SelectedCreature { get; set; }

        //All races to choose
        public IEnumerable<RaceDto> RaceOptions { get; set; }
    }
}