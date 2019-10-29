using jeanie.Lib;
using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class JobsController : Controller
    {
        [HttpPost]
        public ActionResult SendReminders()
        {
            using (var context = new JeanieContext())
            {
                var reservations = context.Reservations
                    .Where(e => e.Status == ReservationStatus.Complete)
                    .Where(e => DbFunctions.DiffHours(DateTime.Now, e.StartDate) <= 48)
                    .ToList();
                foreach (var reservation in reservations)
                {
                    reservation.Status = ReservationStatus.ReminderSent;
                    Mailer.SendReminder(ControllerContext, new ReservationViewModel(reservation));
                }
                context.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
