using System;
using System.Linq;
using System.Text;

namespace jeanie.Lib
{
    public static class Extensions
    {
        /// <summary>
        /// Truncate string to specific size with ellipses.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Preview(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (value.Length > length)
            {
                return value.Substring(0, length) + "...";
            }

            return value;
        }

        /// <summary>
        /// Returns the name of the enum in string form with spaces.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Text(this Enum value)
        {
            return new string(value.ToString()
                .SelectMany(c =>
                    char.IsUpper(c)
                    ? new[] { ' ', c }
                    : new[] { c })
                .ToArray()).Trim();
        }

        /// <summary>
        /// Returns the first word capitalized.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstWord(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var updatedWord = new StringBuilder();
            var words = value.Split(' ');
            for (int i = 0; i < words[0].Length; i++)
            {
                if (i == 0)
                    updatedWord.Append(char.ToUpper(words[0][i]));
                else
                    updatedWord.Append(char.ToLower(words[0][i]));
            }

            return updatedWord.ToString();
        }
    }
}