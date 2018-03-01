using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using RdwTechdayRegistration.Services;

namespace RdwTechdayRegistration.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Welkom bij de RDW Techday!",
                $"Om te kunnen inloggen, dient uw email adres nog bevestigd te worden. Dit doet u door op de volgende <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> te klikken");
        }
    }
}
