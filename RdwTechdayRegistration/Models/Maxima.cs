using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    public class Maxima
    {
        public Maxima()
        {
            ForceChangeCount = 0;
        }
        public int Id { get; set; }
        [Display(Name = "Maximum aantal RDW'ers")]
        public int MaxRDW { get; set; }
        [Display(Name = "Maximum aantal niet RDW'ers")]
        public int MaxNonRDW { get; set; }

        [Display(Name = "Mutaties blokkeren")]
        public bool SiteHasBeenLocked { get; set; }


        // this field is used to create a dummy change so this record is forcibly saved
        // to trigger a concurrency check in register session
        public int ForceChangeCount { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
