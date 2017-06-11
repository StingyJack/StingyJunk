namespace StingyJunk
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     A case insensitive string keyed dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OicDic<T> : Dictionary<string, T>
    {
        /// <summary>
        ///     Creates a new instance 
        /// </summary>
        public OicDic() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}