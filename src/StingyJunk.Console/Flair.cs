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

        public static Flair Warning => _warningFlair;
        public static Flair Success => _successFlair;
        public static Flair Error => _errorFlair;
        public static Flair Log => _logFlair;

        private static readonly Flair _warningFlair = new Flair(ConsoleColor.Yellow, ConsoleColor.Black);
        private static readonly Flair _successFlair = new Flair(ConsoleColor.Green, ConsoleColor.Black);
        private static readonly Flair _errorFlair = new Flair(ConsoleColor.Red, ConsoleColor.Black);
        private static readonly Flair _logFlair = new Flair(ConsoleColor.Gray, ConsoleColor.Black);

    }
}