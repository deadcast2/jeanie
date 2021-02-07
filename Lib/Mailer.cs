using jeanie.Areas.Admin.Models;
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
        public static string DefaultEmail => GetSystemVariable("DEFAULT_EMAIL");

        private static List<Attachment> DefaultAttachments
        {
            get
            {
                return new List<Attachment>
                {
                    new Attachment
                    {
                        Filename = "IRB2017-1063_ConsentForm_Parent_20190620_Revised.pdf",
                        Type = "application/pdf",
                        Content = FileHelpers.Base64Encode("~/Content/attachments/IRB2017-1063_ConsentForm_Parent_20190620_Revised.pdf")
                    },
                    new Attachment
                    {
                        Filename = "ParkingMap.pdf",
                        Type = "application/pdf",
                        Content = FileHelpers.Base64Encode("~/Content/attachments/ParkingMap.pdf")
                    },
                    new Attachment
                    {
                        Filename = "Q&A Flyer.docx",
                        Type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        Content = FileHelpers.Base64Encode("~/Content/attachments/Q&A Flyer.docx")
                    }
                };
            }
        }

        public static void SendCompleteAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_CompleteEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "Reservation complete! ✔️", body));

            body = ViewHelpers.RenderToString(context, "_CompleteThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation complete! ✔️", body, DefaultAttachments));
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
            Task.Run(() => Send(reservation.Email, "Reservation confirmed! 🎉", body, DefaultAttachments));
        }

        public static void SendCancellationAlert(ControllerContext context, ReservationViewModel reservation)
        {
            var body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(DefaultEmail, "FWD: Reservation cancelled! ❌", body));

            body = ViewHelpers.RenderToString(context, "_CancelledThankYouEmail", reservation);
            Task.Run(() => Send(reservation.Email, "Reservation cancelled! ❌", body));
        }

        public static void SendReservation(EmailViewModel email)
        {
            Task.Run(() => Send(email.To, "Reservation from Jeanie", email.Body, DefaultAttachments));
            Task.Run(() => Send(DefaultEmail, "FWD: Reservation from Jeanie", email.Body, DefaultAttachments));
        }

        private static async Task Send(string to, string subject, string body, List<Attachment> attachments)
        {
            var apiKey = GetSystemVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var message = new SendGridMessage
            {
                From = new EmailAddress("noreply@jeanie-reservation-system.com", "Jeanie Reservation System"),
                ReplyTo = new EmailAddress(DefaultEmail, "BioSocial Development Lab"),
                Subject = subject,
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
