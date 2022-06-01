using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameApplication.Models.ViewModels
{
    public class DetailsRace
    {
        public RaceDto SelectedRace { get; set; }
        public IEnumerable<CreatureDto> RelatedCreatures { get; set; }
    }
}