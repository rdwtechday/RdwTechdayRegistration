﻿/*  Copyright (C) 2018, RDW 
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RdwTechdayRegistration.Models
{
    public class Track
    {
        public int Id { get; set; }
        [Display(Name = "Track")]
        public string Naam { get; set; }

        /// <summary>
        /// The RGB colorcode to show in the screen
        /// </summary>
        [Display(Name = "RGB Kleurcode")]
        public string Rgb { get; set; }

        public List<Sessie> Sessies { get; set; }
        //public List<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }
        //public List<TrackTijdvak> Tijdvakken { get; set; }
    }
}