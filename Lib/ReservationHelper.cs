using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace jeanie.Lib
{
    public static class ReservationHelper
    {
        public const int HoursInAdvance = 72;

        private const int StartHour = 9;
        private const int EndHour = 21;
        private const int TimeSlotSize = 3;
        private const double TimeIncreament = 0.5;

        public static List<(DateTime start, DateTime end)> GetReservationsForDay(DateTime day)
        {
            return GetReservationsForRange(day, day.AddDays(1));
        }

        public static List<(DateTime start, DateTime end)> GetReservationsForRange(DateTime start, DateTime end)
        {
            using (var context = new JeanieContext())
            {
                var reservations = context.Reservations
                    .Where(e => DbFunctions.TruncateTime(e.StartDate) >= start
                    && DbFunctions.TruncateTime(e.EndDate) <= end).ToList()
                    .Select(e => (e.StartDate.Value, e.EndDate.Value)).ToList();

                var blockedDates = context.BlockedDates
                    .Where(e => e.Date >= start && e.Date <= end).ToList()
                    .Select(e => (e.Date.AddHours(StartHour), e.Date.AddHours(EndHour))).ToList();

                return reservations.Concat(blockedDates).ToList();
            }
        }

        public static List<(DateTime start, DateTime end)> GetTimeSlots(DateTime day)
        {
            var slots = new List<(DateTime start, DateTime end)>();

            for (int i = 0; i < (EndHour - StartHour) * 1 / TimeIncreament; i++)
            {
                var start = day.Date.AddHours(StartHour).AddMinutes(60 * TimeIncreament * i);
                var end = day.Date.AddHours(StartHour + TimeSlotSize).AddMinutes(60 * TimeIncreament * i);

                if (end <= day.Date.AddHours(EndHour))
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
            return (day - DateTime.Today).TotalHours >= HoursInAdvance;
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
