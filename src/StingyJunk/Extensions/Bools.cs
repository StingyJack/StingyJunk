namespace StingyJunk
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Bools
    {
        public static bool OneAndOnlyOne(this IEnumerable<bool> bools)
        {
            return 1== bools.Count(b => b == true);
        }

        //one or none

        //none

        //all
    }
}