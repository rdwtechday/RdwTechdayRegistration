using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Models
{
    public class TrackTijdvak
    {
        public int TrackID { get; set; }
        public int TijdvakID { get; set; }
        public Track Track { get; set; }
        public Tijdvak Tijdvak { get; set; }
    }
}
