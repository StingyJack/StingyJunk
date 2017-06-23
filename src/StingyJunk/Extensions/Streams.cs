namespace StingyJunk.Extensions
{
    using System.IO;

    public static class Streams
    {

        public static byte[] ToBytes(this Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

      
    }
}
