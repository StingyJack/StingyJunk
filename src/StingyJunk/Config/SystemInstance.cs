// ReSharper disable ClassNeverInstantiated.Global
namespace StingyJunk.Config
{
    /// <summary>
    ///      A System Instance
    /// </summary>
    public class SystemInstance
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemInstance"/> class.
        /// </summary>
        public SystemInstance()
        {
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemInstance" /> class.
        /// </summary>
        /// <param name="instanceUrl">if set to <c>true</c> [automatic load instance meta data].</param>
        /// <param name="instanceUrl">The instance URL.</param>
        public SystemInstance(bool autoLoadInstanceMetaData, string instanceUrl)
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
        public string InstanceUrl { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic load instance meta data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic load instance meta data]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadInstanceMetaData { get; }
    }
}