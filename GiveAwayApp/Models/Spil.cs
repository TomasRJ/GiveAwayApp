using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GiveAwayApp.Areas.Identity.Data;

namespace GiveAwayApp.Models
{
    public class Spil
    {
        public int SpilId { get; set; }
        public int SteamId { get; set; }
        public string Titel { get; set; }

        [Display(Name = "Spil Cover URL")]
        public string SpilCoverUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime Udgivelsesdato { get; set; }
        public ulong ValgtAntal { get; set; }
        public ICollection<GiveAwayAppUser> Brugere { get; set; }
        public string Genre { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Pris { get; set; }
    }
}
