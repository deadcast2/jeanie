using jeanie.Lib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jeanie.Controllers
{
    public class DisabledDatesController : Controller
    {
        [HttpGet]
        public ActionResult Show(DateTime day)
        {
            using (var context = new JeanieContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var disabledDates = new List<string>();
                var dayCount = DateTime.DaysInMonth(day.Year, day.Month);
                var monthEnd = day.Date.AddDays(dayCount);
                var bookedTimeSlots = context.Reservations
                    .Where(e => DbFunctions.TruncateTime(e.StartDate) >= day.Date)
                    .Where(e => DbFunctions.TruncateTime(e.EndDate) <= monthEnd.Date)
                    .ToList()
                    .Select(e => (e.StartDate.Value, e.EndDate.Value))
                    .ToList();

                for (int i = 0; i < dayCount; i++)
                {
                    var currDay = day.AddDays(i);

                    if (ReservationHelper.IsDayFullyBooked(currDay, bookedTimeSlots))
                    {
                        disabledDates.Add($"[{currDay.Year}, {currDay.Month - 1}, {currDay.Day}]");
                    }
                }

                return Content($"[{string.Join(", ", disabledDates)}]");
            }
        }
    }
}
