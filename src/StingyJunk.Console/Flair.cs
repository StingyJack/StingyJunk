namespace StingyJunk.Console
{
    using System;

    public class Flair
    {
        public Flair(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }

        public static Flair Warning { get; } = new Flair(ConsoleColor.Yellow, ConsoleColor.Black);

        public static Flair Success { get; } = new Flair(ConsoleColor.Green, ConsoleColor.Black);

        public static Flair Error { get; } = new Flair(ConsoleColor.Red, ConsoleColor.Black);

        public static Flair Log { get; } = new Flair(ConsoleColor.Gray, ConsoleColor.Black);
    }
}