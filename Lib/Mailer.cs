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

        public static void SendCompleteAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_CompleteEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "Reservation complete! ✔️", body));

            body = ViewHelpers.RenderToString(context, "_CompleteThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation complete! ✔️", body));
        }

        public static void SendReminder(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_ReminderEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation reminder ⏰", body));
        }

        public static void SendConfirmationAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_ConfirmedThankYouEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "FWD: Reservation confirmed! 🎉", body));

            body = ViewHelpers.RenderToString(context, "_ConfirmedThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation confirmed! 🎉", body));
        }

        public static void SendCancellationAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "FWD: Reservation cancelled ❌", body));

            body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation cancelled ❌", body));
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
