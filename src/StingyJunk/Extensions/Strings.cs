namespace StingyJunk.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class Strings
    {
        public static string ToNewLineList(this IEnumerable<string> list)
        {
            return string.Join(Environment.NewLine, list);
        }

        public static string ToCsl(this IEnumerable<string> list)
        {
            return string.Join(",", list);
        }
    }
}