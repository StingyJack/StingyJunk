namespace ConsoleHelpers
{
    using System;

    public class ConsoleHelpers
    {
        public int? MaxLines { get; set; }

        public ConsoleHelpers(int? maxLines = null)
        {
            MaxLines = maxLines;
        }

        public void Cw(char character, ConsoleColor? color = null)
        {
            var def = Console.ForegroundColor;

            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }
            Console.Write(character);
            if (color.HasValue)
            {
                Console.ForegroundColor = def;
            }
        }


        public void Cwl(string message, ConsoleColor? color = null)
        {
            var def = Console.ForegroundColor;

            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }
            Console.WriteLine(message);
            if (color.HasValue)
            {
                Console.ForegroundColor = def;
            }
        }
    }
}