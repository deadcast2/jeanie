using jeanie.Lib;
using jeanie.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class JobsController : Controller
    {
        private int HoursNotice => int.Parse(ConfigurationManager.AppSettings["HoursNotice"]);

        [HttpPost]
        public ActionResult SendReminders()
        {
            using (var context = new JeanieContext())
            {
                var reservations = context.Reservations
                    .Where(e => e.Status == ReservationStatus.Complete)
                    .Where(e => DbFunctions.DiffHours(DateTime.Now, e.StartDate) <= HoursNotice)
                    .ToList();
                foreach (var reservation in reservations)
                {
                    reservation.Status = ReservationStatus.ReminderSent;
                    reservation.UpdatedAt = DateTime.Now;
                    Mailer.SendReminder(ControllerContext, new ReservationViewModel(reservation));
                }
                context.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
