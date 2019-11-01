using System;
using System.Text.RegularExpressions;

namespace MroczekDotDev.Sfira.Extensions.System
{
    public static class StringExtensions
    {
        public static string ToHypertext(this string text)
        {
            text = Regex.Replace(text, @"#(\w+)", @"<a href=""/tag/$1"">#$1</a>");
            text = text.Replace(Environment.NewLine, "<br>");
            return text;
        }

        public static string ToWebUrl(this string url)
        {
            if (!Regex.IsMatch(url, "^https?://.*"))
            {
                url = "http://" + url;
            }

            return url;
        }
    }
}
