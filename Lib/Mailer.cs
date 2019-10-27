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
        private static string DefaultEmail => GetSystemVariable("DEFAULT_EMAIL");

        public static async Task SendConfirmationAlert(ControllerContext context, ReservationViewModel reservation)
        {
            await Send(DefaultEmail, "Reservation confirmed! 🎉",
                ViewHelpers.RenderToString(context, "_ReservationConfirmationEmail", reservation));
        }

        public static async Task SendThankYou(ControllerContext context, ReservationViewModel reservation)
        {
            await Send(reservation.Email, "Reservation confirmed! 🎉",
                ViewHelpers.RenderToString(context, "_ThankYouEmail", reservation));
        }

        private static async Task Send(string to, string subject, string body)
        {
            var apiKey = GetSystemVariable("SENDGRID_API_KEY");
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

        private static string GetSystemVariable(string name)
        {
#if DEBUG
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
#else
            return Environment.GetEnvironmentVariable(name);
#endif
        }
    }
}
