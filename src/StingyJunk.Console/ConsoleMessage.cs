namespace StingyJunk.Console
{
    internal class ConsoleMessage
    {
        /// <summary>
        /// The text to write
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     The style to write the text in
        /// </summary>
        public Flair Flair { get; set; }

        /// <summary>
        ///     The position to write the text at
        /// </summary>
        public Position WritePosition { get; set; }

        public ConsoleMessage(string message, Flair flair, Position writePosition)
        {
            Message = message;
            Flair = flair;
            WritePosition = writePosition;
        }
    }
}