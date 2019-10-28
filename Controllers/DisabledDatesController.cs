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
            const int dayMargin = 14;

            using (var context = new JeanieContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var disabledDates = new List<string>();
                var monthStart = day.Date.AddDays(-dayMargin);
                var monthEnd = day.Date.AddDays(DateTime.DaysInMonth(day.Year, day.Month) + dayMargin);
                var dayCount = (monthEnd - monthStart).Days;
                var bookedTimeSlots = ReservationHelper.GetReservationsForRange(monthStart, monthEnd);

                for (int i = 0; i < dayCount; i++)
                {
                    var currDay = monthStart.AddDays(i);

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
