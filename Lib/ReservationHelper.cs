using jeanie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace jeanie.Lib
{
    public static class ReservationHelper
    {
        public static int HoursInAdvance => int.Parse(ConfigurationManager.AppSettings["HoursInAdvance"]);
        private static double StartHour => double.Parse(ConfigurationManager.AppSettings["StartHour"]);
        private static double EndHour => double.Parse(ConfigurationManager.AppSettings["EndHour"]);
        private static int TimeSlotSize => int.Parse(ConfigurationManager.AppSettings["TimeSlotSize"]);
        private static double TimeIncreament => double.Parse(ConfigurationManager.AppSettings["TimeIncreament"]);

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
                    .Where(e => e.StartDate >= start && e.EndDate <= end).ToList()
                    .Select(e => (e.StartDate, e.EndDate)).ToList();

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
            return openTimeSlots.Where(e => IsAvailableTimeSlot(bookedTimeSlots, e)).ToList();
        }

        public static bool IsDayFullyBooked(DateTime day, List<(DateTime start, DateTime end)> bookedTimeSlots,
            Setting setting = null)
        {
            var dailyLimitReached = bookedTimeSlots.Where(b => b.start.Date == day.Date).Count() >= setting?.DailyReservationLimit;
            return dailyLimitReached || AvailableTimeSlots(day, bookedTimeSlots).Count == 0 || day >= DateTime.Parse("08/01/2022");
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

        public static bool IsValidTimeSlot((DateTime start, DateTime end) timeSlot)
        {
            return GetTimeSlots(timeSlot.start).Contains(timeSlot);
        }

        public static bool IsAvailableTimeSlot(List<(DateTime start, DateTime end)> bookedTimeSlots,
            (DateTime start, DateTime end) newTimeSlot)
        {
            var valid = IsValidTimeSlot(newTimeSlot);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.start >= e.start && newTimeSlot.start <= e.end);
            valid &= !bookedTimeSlots.Any(e => newTimeSlot.end >= e.start && newTimeSlot.end < e.end);
            return valid;
        }

        public static List<FormattedReservation> FromDataTable(DataTable dataTable, out int total, out int filtered)
        {
            using (var context = new JeanieContext())
            {
                var reservations = context.FormattedReservations.AsQueryable();
                total = reservations.Count();
                filtered = total;

                if (!string.IsNullOrWhiteSpace(dataTable.search.value))
                {
                    reservations = reservations.Where(r =>
                        r.Name.Contains(dataTable.search.value) ||
                        r.Grade.Contains(dataTable.search.value) ||
                        r.TimeSlot.Contains(dataTable.search.value) ||
                        r.Status.Contains(dataTable.search.value) ||
                        r.CreatedAt.Contains(dataTable.search.value)
                    );
                    filtered = reservations.Count();
                }

                foreach (var order in dataTable.order)
                {
                    switch (dataTable.columns[order.column].name)
                    {
                        case nameof(FormattedReservation.Name):
                            reservations = order.IsAsc ? reservations.OrderBy(r => r.Name)
                                : reservations.OrderByDescending(r => r.Name);
                            break;
                        case nameof(FormattedReservation.Grade):
                            reservations = order.IsAsc ? reservations.OrderBy(r => r.Grade)
                                : reservations.OrderByDescending(r => r.Grade);
                            break;
                        case nameof(FormattedReservation.TimeSlot):
                            reservations = order.IsAsc ? reservations.OrderBy(r => r.StartDate)
                                : reservations.OrderByDescending(r => r.StartDate);
                            break;
                        case nameof(FormattedReservation.Status):
                            reservations = order.IsAsc ? reservations.OrderBy(r => r.RawStatus)
                                : reservations.OrderByDescending(r => r.RawStatus);
                            break;
                        case nameof(FormattedReservation.CreatedAt):
                            reservations = order.IsAsc ? reservations.OrderBy(r => r.RawCreatedAt)
                                : reservations.OrderByDescending(r => r.RawCreatedAt);
                            break;
                    }
                }

                reservations = reservations.Skip(dataTable.start).Take(dataTable.length);

                return reservations.ToList();
            }
        }
    }
}
