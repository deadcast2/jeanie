using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jeanie.Lib
{
    public static class Extensions
    {
        public static string Preview(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if(value.Length > length)
            {
                return value.Substring(0, length) + "...";
            }

            return value;
        }

        public static string Text(this Enum value)
        {
            return new string(value.ToString()
                .SelectMany(c =>
                    char.IsUpper(c)
                    ? new[] { ' ', c }
                    : new[] { c })
                .ToArray()).Trim();
        }
    }
}