using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Models
{
    public class Maxima
    {
        public int Id { get; set; }
        [Display(Name = "Maximum aantal RDW'ers")]
        public int MaxRDW { get; set; }
        [Display(Name = "Maximum aantal niet RDW'ers")]
        public int MaxNonRDW { get; set; }
    }
}
