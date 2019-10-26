using jeanie.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Lib
{
    public static class Mailer
    {
        private static string DefaultEmail =>
            Environment.GetEnvironmentVariable("DEFAULT_EMAIL", EnvironmentVariableTarget.Machine);

        public static async Task SendReservationConfirmation(ControllerContext context, ReservationViewModel reservation)
        {
            await Send(DefaultEmail, "Reservation confirmed! 🎉",
                ViewHelpers.RenderToString(context, "_ReservationConfirmationEmail", reservation));
        }

        private static async Task Send(string to, string subject, string body)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.Machine);
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage
            {
                From = new EmailAddress("jeanie-reservation-system@outlook.com", "Jeanie Reservation System"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };
            message.AddTo(to);
            await client.SendEmailAsync(message);
        }
    }
}
