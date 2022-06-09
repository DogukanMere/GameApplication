using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameApplication.Models.ViewModels
{
    public class DetailsCreature
    {
        public CreatureDto SelectedCreature { get; set; }
        public IEnumerable<CreatureDto> TryCreature { get; set; }
        public IEnumerable<DungeonDto> LiveinDungeons { get; set; }
        public IEnumerable<DungeonDto> AvailableDungeons { get; set; }
    }
}