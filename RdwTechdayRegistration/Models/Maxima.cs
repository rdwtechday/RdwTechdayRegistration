/*  Copyright (C) 2018, RDW 
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/.  
 *  
 */

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
