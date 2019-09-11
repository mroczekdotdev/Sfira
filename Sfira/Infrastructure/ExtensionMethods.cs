using System;
using System.Text.RegularExpressions;

namespace MroczekDotDev.Sfira.Infrastructure
{
    public static class ExtensionMethods
    {
        public static string Relative(this DateTime dateTime)
        {
            const int secondsInOneMinute = 60;
            const int secondsInOneHour = 3600;
            const int secondsInOneDay = 86400;
            const int secondsInThirtyDays = 2592000;

            TimeSpan delta = DateTime.Now.Subtract(dateTime);

            switch (delta.TotalSeconds)
            {
                case var v when v < secondsInOneMinute:
                    return delta.ToString("%s") + " seconds ago";

                case var v when v < secondsInOneMinute * 2:
                    return delta.ToString("%m") + " minute ago";

                case var v when v < secondsInOneHour:
                    return delta.ToString("%m") + " minutes ago";

                case var v when v < secondsInOneHour * 2:
                    return delta.ToString("%h") + " hour ago";

                case var v when v < secondsInOneDay:
                    return delta.ToString("%h") + " hours ago";

                case var v when v < secondsInOneDay * 2:
                    return delta.ToString("%d") + " day ago";

                case var v when v < secondsInThirtyDays:
                    return delta.ToString("%d") + " days ago";

                default:
                    return dateTime.ToString("MMMM d, yyyy");
            }
        }

        public static string FormatAsMessage(this string message)
        {
            message = Regex.Replace(message, @"#(\w+)", @"<a href=""/tag/$1"">#$1</a>");
            message = message.Replace(Environment.NewLine, "<br>");
            return message;
        }
    }
}
