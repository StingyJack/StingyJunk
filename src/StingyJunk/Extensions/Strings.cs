namespace StingyJunk.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Strings
    {
        public static string GetCommaSepListOfPropNames(this object instance)
        {
            var props = new StringBuilder();
            foreach (var prop in instance.GetType().GetProperties())
            {
                props.Append($"{prop.Name},");
            }
            props.Length--;
            return props.ToString();
        }

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