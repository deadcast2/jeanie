using jeanie.Models;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace jeanie.Lib
{
    public static class Calendar
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

        public static string FormatDate(DateTime date)
        {
            return Regex.Replace(date.ToString("o"), "-|:|\\.\\d{7}", "");
        }
    }
}
