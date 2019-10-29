using jeanie.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            var attachments = new List<Attachment>
            {
                new Attachment
                {
                    Filename="Best Route to Human Sciences Building.JPG",
                    Type = "image/jpeg",
                    Content = FileHelpers.Base64Encode("~/Content/attachments/Best Route to Human Sciences Building.JPG")
                },
                new Attachment
                {
                    Filename="IRB2017-1063_ConsentForm_Parent_20190620_Revised.pdf",
                    Type = "application/pdf",
                    Content = FileHelpers.Base64Encode("~/Content/attachments/IRB2017-1063_ConsentForm_Parent_20190620_Revised.pdf")
                },
                new Attachment
                {
                    Filename="ParkingMap.pdf",
                    Type = "application/pdf",
                    Content = FileHelpers.Base64Encode("~/Content/attachments/ParkingMap.pdf")
                },
                new Attachment
                {
                    Filename="Q&A Flyer.docx",
                    Type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    Content = FileHelpers.Base64Encode("~/Content/attachments/Q&A Flyer.docx")
                }
            };
            body = ViewHelpers.RenderToString(context, "_ConfirmedThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation confirmed! 🎉", body, attachments));
        }

        public static void SendCancellationAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "FWD: Reservation cancelled! ❌", body));

            body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation cancelled! ❌", body));
        }

        private static async Task Send(string to, string subject, string body, List<Attachment> attachments)
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

            if (attachments.Count > 0)
                message.AddAttachments(attachments);

            await client.SendEmailAsync(message);
        }

        private static async Task Send(string to, string subject, string body)
        {
            await Send(to, subject, body, new List<Attachment>());
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
