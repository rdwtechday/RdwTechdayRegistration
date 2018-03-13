using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string name, string email, string link)
        {
            string plainMessage = $"Beste {name}" + Environment.NewLine + Environment.NewLine +
                $"Om te kunnen inloggen, dient uw email adres nog bevestigd te worden. Dit doet u door op de volgende link te klikken: {HtmlEncoder.Default.Encode(link)}" +
                $"Met vriendelijke groet," + Environment.NewLine + Environment.NewLine +
                $"RDW Techday";

            string htmlMessage = $"Beste {name},<br/><br/>"+
                $"Om te kunnen inloggen, dient uw email adres nog bevestigd te worden. Dit doet u door op de volgende <a href='{HtmlEncoder.Default.Encode(link)}'>link</a> te klikken."+
                $"<br/><br/><br/>Met vriendelijke groet,<br/><br/>RDW Techday";

            return emailSender.SendEmailAsync(email, "Welkom bij de RDW Techday!", plainMessage, htmlMessage);
        }
    }
}