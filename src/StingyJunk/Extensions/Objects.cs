namespace StingyJunk.Extensions
{
    using System.Text;

    public static class Objects
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
    }
}