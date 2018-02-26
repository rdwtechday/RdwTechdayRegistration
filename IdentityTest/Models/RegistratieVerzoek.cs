using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using RdwTechdayRegistration.ValidationHelpers;

namespace RdwTechdayRegistration.Models
{
    public enum RegistratieState
    {
        Requested, Accepted, Denied
    }

    
    public class RegistratieVerzoek
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een naam in")]
        public string Naam { get; set; }
        public string Organisatie { get; set; }
        [Required(ErrorMessage = "Vul het e-mail adres in")]
        [Remote(action: "VerifyEmail", controller: "RegistratieVerzoeken")]
        public string Email { get; set; }
        public string Telefoon { get; set; }
        public Deelnemer Deelnemer { get; set; }





    }
}