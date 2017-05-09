namespace StingyJunk.Compilation.Misc
{
    using System.ComponentModel;

    public enum CompileDirection
    {
        EverythingBuiltAsClassesAndReffed,
        OnlyClassesBuiltAsScriptsAndReffed
    }

    /// <summary>
    ///     The type of script directive
    /// </summary>
    public enum DirectiveType
    {
        /// <summary>
        ///     Load the following script into the global scope
        /// </summary>
        [Description("#load")] ScriptRef,

        /// <summary>
        ///     Load the assembly into the global scope
        /// </summary>
        [Description("#r")] AssemblyRef
    }

    public enum ResolutionTargetType
    {
        Unknown,
        Cs,
        Csx,
        Other
    }


    /// <summary>
    ///     The assembly resolution GET method
    /// </summary>
    public enum AsmGet
    {
        /// <summary>
        ///     The entry assembly
        /// </summary>
        Entry,

        /// <summary>
        ///     The calling assembly
        /// </summary>
        Calling,

        /// <summary>
        ///     The executing assembly
        /// </summary>
        Executing,

        /// <summary>
        ///     The named assembly
        /// </summary>
        Named
    }

    public enum AccMod
    {
        Public,
        Protected,
        ProtectedInternal,
        Internal,
        Private
    }
}