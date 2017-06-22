namespace StingyJunk.Config
{
    using System.Collections.Generic;

    //TODO: Change this to SystemInstances and move tfs specific stuff to .Tfs assembly 
    public class SystemInstances
    {
        /// <summary>
        ///     Base instance urls
        /// </summary>
        public List<SystemInstance> Instances { get; } = new List<SystemInstance>();

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