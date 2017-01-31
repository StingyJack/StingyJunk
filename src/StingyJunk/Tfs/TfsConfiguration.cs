namespace StingyJunk.Tfs
{
    using System.Collections.Generic;

    public class TfsConfiguration
    {
        /// <summary>
        ///     Base instance urls
        /// </summary>
        public List<TfsInstance> Instances { get; set; } = new List<TfsInstance>();

        public int MaxRecordsInResultSets { get; set; }

        /// <summary>
        ///     True if the instance is configured
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Instances == null || Instances.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}