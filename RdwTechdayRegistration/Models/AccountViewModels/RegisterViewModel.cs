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

namespace RdwTechdayRegistration.Models.AccountViewModels
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        [EmailAddress]
        [Display(Name = "RDW e-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Voor- en Achternaam")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password, gebruik hiervoor niet uw RDW wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Herhaal password")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Divisie")]
        public string Department { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!EmailValidator.IsValid(Email))
            {
                yield return new ValidationResult(
                    $"Vul een e-mail adres in", new[] { "Email" });
            }
        }

    }
}
