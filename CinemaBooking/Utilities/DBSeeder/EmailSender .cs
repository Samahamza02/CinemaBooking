using Microsoft.AspNetCore.Identity.UI.Services;

using System.Net;
using System.Net.Mail;

namespace CinemaBooking.Utilities.DBSeeder
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("semsemhamza3@gmail.com", "xkdc gjom fefn vvfm")
            };

            return client.SendMailAsync(
                new MailMessage(from: "semsemhamza3@gmail.com", to: email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                });
        }
    }
}
