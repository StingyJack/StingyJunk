// ReSharper disable ClassNeverInstantiated.Global
namespace StingyJunk.Tfs
{
    /// <summary>
    ///      A Tfs Instance
    /// </summary>
    public class TfsInstance
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TfsInstance"/> class.
        /// </summary>
        public TfsInstance()
        {
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TfsInstance" /> class.
        /// </summary>
        /// <param name="autoLoadInstanceMetaData">if set to <c>true</c> [automatic load instance meta data].</param>
        /// <param name="instanceUrl">The instance URL.</param>
        public TfsInstance(bool autoLoadInstanceMetaData, string instanceUrl)
        {
            AutoLoadInstanceMetaData = autoLoadInstanceMetaData;
            InstanceUrl = instanceUrl;
        }

        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        /// <value>
        /// The name of the instance.
        /// </value>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the instance URL.
        /// </summary>
        /// <value>
        /// The instance URL.
        /// </value>
        public string InstanceUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic load instance meta data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic load instance meta data]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadInstanceMetaData { get; set; }
    }
}