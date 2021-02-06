using jeanie.Lib;
using jeanie.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Models
{
    public class EmailViewModel
    {
        private Setting Setting;
        private Reservation Reservation;

        public string To { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public EmailViewModel() { }

        public EmailViewModel(Setting setting, Reservation reservation)
        {
            Setting = setting ?? throw new Exception("Setting cannot be null");
            Reservation = reservation ?? throw new Exception("Reservation cannot be null");
        }

        public string GetBody()
        {
            var url = new ReservationViewModel { Id = Reservation.Id }.Url;
            var name = Reservation.Name.FirstWord();

            return Setting.EmailTemplate.Replace("$name", name).Replace("$link", $"<a href=\"{url}\">{url}</a>");
        }

        public void Send()
        {
            Mailer.SendReservation(this);
        }
    }
}
