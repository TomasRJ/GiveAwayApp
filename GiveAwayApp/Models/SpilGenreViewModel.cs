using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GiveAwayApp.Models
{
    public class SpilGenreViewModel
    {
        public List<Spil> SpilList { get; set; }
        public SelectList Genre { get; set; }
        public string SpilGenre { get; set; }
        public string TitelFilter { get; set; }
    }
}
