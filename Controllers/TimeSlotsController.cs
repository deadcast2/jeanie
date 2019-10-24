using jeanie.Lib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class TimeSlotsController : Controller
    {
        [HttpGet]
        public ActionResult Show(DateTime day)
        {
            using (var context = new JeanieContext())
            {
                var bookedTimeSlots = context.Reservations
                    .Where(e => day >= DbFunctions.TruncateTime(e.StartDate)
                    && day <= DbFunctions.TruncateTime(e.EndDate)).ToList();
                var availableTimeSlot = ReservationHelper.TimeSlotsAvailable(day, bookedTimeSlots);
                return Json(availableTimeSlot.Select(e => new
                {
                    text = $"{e.start.ToShortTimeString()} - {e.end.ToShortTimeString()}",
                    value = $"{e.start.Hour}-{e.end.Hour}"
                }), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
