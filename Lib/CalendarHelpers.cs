using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using jeanie.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace jeanie.Lib
{
    public static class CalendarHelpers
    {
        private static string Text => "BioSocial Interplay Development Lab Meeting";

        private static string Location => "Development Building, Lubbock, TX 79410";

        public static string GoogleLink(ReservationViewModel reservation)
        {
            var start = FormatDate(reservation.StartDate.Value);
            var end = FormatDate(reservation.EndDate.Value);
            var query = $"action=TEMPLATE&text={HttpUtility.UrlEncode(Text)}&dates={start}/{end}&location={HttpUtility.UrlEncode(Location)}";
            return $"https://calendar.google.com/calendar/render?{query}";
        }

        public static string OutlookLink(ReservationViewModel reservation)
        {
            return $"{reservation.BaseUrl}/reservations/download/{reservation.Id}";
        }

        public static string AppleLink(ReservationViewModel reservation)
        {
            return OutlookLink(reservation);
        }

        public static string FormatDate(DateTime date)
        {
            return Regex.Replace(date.ToString("o"), "-|:|\\.\\d{7}", "");
        }

        public static byte[] File(ReservationViewModel reservation)
        {
            var calendar = new Calendar();
            calendar.Events.Add(new CalendarEvent
            {
                Start = new CalDateTime(reservation.StartDate.Value),
                End = new CalDateTime(reservation.EndDate.Value),
                Summary = Text,
                Location = Location
            });
            var contents = new CalendarSerializer().SerializeToString(calendar);
            return Encoding.ASCII.GetBytes(contents);
        }
    }
}
