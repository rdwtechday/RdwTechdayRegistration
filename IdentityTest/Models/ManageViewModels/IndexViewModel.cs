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

using RdwTechdayRegistration.ValidationHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models.ManageViewModels
{
    public class IndexViewModel 
    {

        [Display(Name= "Login")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public bool isRdw { get; set; }

        [Required]
        [Display(Name = "Voor- en Achternaam")]
        public string Name{ get; set; }

        [Phone]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Divisie")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Organisatie")]
        public string Organisation { get; set; }

        public string StatusMessage { get; set; }

    }
}
