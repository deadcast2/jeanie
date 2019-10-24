using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jeanie.Lib
{
    public static class ReservationHelper
    {
        private static readonly int StartTime = 9;
        private static readonly int EndTime = 21;
        private static readonly int SlotSize = 3;

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
            return openTimeSlots.Where(e => !bookedTimeSlots.Contains(e)).ToList();
        }

        public static bool IsDayFullyBooked(DateTime day, List<(DateTime start, DateTime end)> bookedTimeSlots)
        {
            return FilterTimeSlots(bookedTimeSlots, GetTimeSlots(day)).Count == 0;
        }

        public static bool IsValidTimeSlot(List<(DateTime start, DateTime end)> bookedTimeSlots, 
            (DateTime start, DateTime end) newTimeSlot)
        {
            var valid = GetTimeSlots(newTimeSlot.start).Contains(newTimeSlot);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.start >= e.start && newTimeSlot.start < e.end);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.end >= e.start && newTimeSlot.end < e.end);
            return valid;
        }
    }
}
