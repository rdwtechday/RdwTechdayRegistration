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

using RdwTechdayRegistration.Models;
using System.Collections.Generic;

namespace RdwTechdayRegistration.Models
{
    public class Deelnemer
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Organisatie { get; set; }
        public string Email { get; set; }
        public string Telefoon { get; set; }
    }
}