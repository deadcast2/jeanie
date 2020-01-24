using jeanie.Areas.Admin.Models;
using jeanie.Lib;
using jeanie.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace jeanie.Areas.Admin.Controllers
{
    [Authorize]
    public class BlockedDatesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dates()
        {
            using (var context = new JeanieContext())
            {
                var groupedDates = context.BlockedDates.GroupBy(e => DbFunctions.TruncateTime(e.StartDate).Value)
                    .ToList().Select(e => new
                    {
                        Date = e.Key,
                        TimeSlots = e.Select(s => (s.StartDate, s.EndDate)).ToList()
                    }).ToList();
                var results = groupedDates.Select(e => new
                {
                    Date = e.Date.ToShortDateString(),
                    IsDayFullyBooked = ReservationHelper.IsDayFullyBooked(e.Date, e.TimeSlots)
                }).ToList();

                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult TimeSlots(DateTime date)
        {
            using (var context = new JeanieContext())
            {
                var blockedDates = context.BlockedDates
                    .Where(e => DbFunctions.TruncateTime(e.StartDate) == date.Date).ToList()
                    .Select(e => (e.StartDate, e.EndDate)).ToList();
                var availableDates = ReservationHelper.AvailableTimeSlots(date.Date, blockedDates);
                return PartialView("_TimeSlots", new BlockedDatesViewModel
                {
                    SelectedDate = date,
                    AvailableSlots = availableDates,
                    BookedSlots = blockedDates
                });
            }
        }

        [HttpPost]
        public ActionResult Update(DateTime start, DateTime end, string action)
        {
            using (var context = new JeanieContext())
            {
                var blockedDate = context.BlockedDates.FirstOrDefault(e => e.StartDate == start && e.EndDate == end);

                if (action == "add" && blockedDate == null && ReservationHelper.IsValidTimeSlot((start, end)))
                {
                    context.BlockedDates.Add(new BlockedDate { StartDate = start, EndDate = end });
                }
                else if (action == "remove" && blockedDate != null)
                {
                    context.Entry(blockedDate).State = EntityState.Deleted;
                }

                if (context.SaveChanges() > 0)
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        public ActionResult BlockWholeDay(DateTime date)
        {
            using (var context = new JeanieContext())
            {
                var allTimeSlots = ReservationHelper.GetTimeSlots(date);
                var minTimeSlot = allTimeSlots.First();
                var maxTimeSlot = allTimeSlots.Last();

                var blockedDate = context.BlockedDates.FirstOrDefault(e => e.StartDate == minTimeSlot.start 
                    && e.EndDate == maxTimeSlot.end);
                if (blockedDate == null)
                {
                    // Remove any blocked time slots for this date.
                    foreach(var timeSlot in context.BlockedDates
                        .Where(e => DbFunctions.TruncateTime(e.StartDate) == date))
                    {
                        context.Entry(timeSlot).State = EntityState.Deleted;
                    }

                    // Block whole date by adding a min and max date from all time slots.
                    context.BlockedDates.Add(new BlockedDate { StartDate = minTimeSlot.start, EndDate = maxTimeSlot.end });
                }

                context.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
