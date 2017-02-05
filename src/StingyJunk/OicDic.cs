namespace StingyJunk
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     An case insensitive ordinal string keyed dictionary
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

        public bool Has(string key)
        {
            return ContainsKey(key);
        }

        public T Get(string key)
        {
            return this[key];
        }
    }
}