namespace StingyJunk.IO.Odd
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum EncryptionAlgorithm
    {
        /// <summary>
        ///     Needs more DES - a Triple 56 bit that really can only use two 56 bits, meaning it can be broken with commodity hardware in 2017. 
        /// </summary>
        DES3,

        /// <summary>
        ///     Super weak crypto. Only included for systems that existed during the 40 bit US export restriction time.
        /// </summary>
        RC2
    }
}