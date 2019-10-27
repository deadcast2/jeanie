using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace jeanie.Lib
{
    public static class ReservationHelper
    {
        public const int HoursInAdvance = 72;

        private const int StartTime = 9;
        private const int EndTime = 21;
        private const int SlotSize = 3;

        public static List<(DateTime start, DateTime end)> GetReservations(DateTime day)
        {
            using (var context = new JeanieContext())
            {
                return context.Reservations
                    .Where(e => day >= DbFunctions.TruncateTime(e.StartDate)
                    && day <= DbFunctions.TruncateTime(e.EndDate)).ToList()
                    .Select(e => (e.StartDate.Value, e.EndDate.Value)).ToList();
            }
        }

        public static List<(DateTime start, DateTime end)> GetTimeSlots(DateTime day)
        {
            var slots = new List<(DateTime start, DateTime end)>();

            for (int i = 0; i < (EndTime - StartTime); i++)
            {
                var start = day.Date.AddHours(StartTime + i);
                var end = day.Date.AddHours(StartTime + SlotSize + i);

                if(end <= day.Date.AddHours(EndTime))
                    slots.Add((start, end));
            }

            return slots;
        }

        public static List<(DateTime start, DateTime end)> FilterTimeSlots(
            List<(DateTime start, DateTime end)> bookedTimeSlots, 
            List<(DateTime start, DateTime end)> openTimeSlots)
        {
            return openTimeSlots.Where(e => IsValidTimeSlot(bookedTimeSlots, e)).ToList();
        }

        public static bool IsDayFullyBooked(DateTime day, List<(DateTime start, DateTime end)> bookedTimeSlots)
        {
            return AvailableTimeSlots(day, bookedTimeSlots).Count == 0;
        }

        public static List<(DateTime start, DateTime end)> AvailableTimeSlots(DateTime day,
            List<(DateTime start, DateTime end)> bookedTimeSlots)
        {
            return FilterTimeSlots(bookedTimeSlots, GetTimeSlots(day));
        }

        public static bool IsValidDate(DateTime day)
        {
            return (day - DateTime.Now.Date).TotalHours >= HoursInAdvance;
        }

        public static bool IsValidTimeSlot(List<(DateTime start, DateTime end)> bookedTimeSlots, 
            (DateTime start, DateTime end) newTimeSlot)
        {
            var valid = GetTimeSlots(newTimeSlot.start).Contains(newTimeSlot);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.start >= e.start && newTimeSlot.start <= e.end);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.end >= e.start && newTimeSlot.end < e.end);
            return valid;
        }
    }
}
