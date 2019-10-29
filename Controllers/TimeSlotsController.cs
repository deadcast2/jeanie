using jeanie.Lib;
using System;
using System.Linq;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class TimeSlotsController : Controller
    {
        [HttpGet]
        public ActionResult Show(DateTime day)
        {
            var availableTimeSlot = ReservationHelper.AvailableTimeSlots(day,
                ReservationHelper.GetReservationsForDay(day));
            return Json(availableTimeSlot.Select(e => new
            {
                text = $"{e.start.ToShortTimeString()} - {e.end.ToShortTimeString()}",
                value = $"{e.start.Hour}-{e.end.Hour}"
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
