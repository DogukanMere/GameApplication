using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameApplication.Models.ViewModels
{
    public class DetailsDungeon
    {
        public DungeonDto SelectedDungeon { get; set; }
        public IEnumerable<CreatureDto> AvailableCreatures { get; set; }
    }
}