namespace StingyJunk.Console
{
    public class Position
    {
        public int Top { get; }
        public int Left { get; }

        public Position(int top, int left)
        {
            Top = top;
            Left = left;
        }

        public override string ToString()
        {
            return $"({Top},{Left})";
        }
    }
}