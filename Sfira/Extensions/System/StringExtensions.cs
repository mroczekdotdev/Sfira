using System;
using System.Text.RegularExpressions;

namespace MroczekDotDev.Sfira.Extensions.System
{
    public static class StringExtensions
    {
        public static string FormatAsEntry(this string entry)
        {
            entry = Regex.Replace(entry, @"#(\w+)", @"<a href=""/tag/$1"">#$1</a>");
            entry = entry.Replace(Environment.NewLine, "<br>");
            return entry;
        }
    }
}
