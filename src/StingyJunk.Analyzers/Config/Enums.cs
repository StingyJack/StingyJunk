namespace StingyJunk.Analyzers.Config
{
    /// <summary>
    ///     The type of match the ident match info represents
    /// </summary>
    public enum MatchType
    {
        Unknown,
        NamespaceName,
        TypeName, /*Class, Interface, Struct, etc? need to be separate designations*/
        MemberName
    }

    public enum ReportAs
    {
        None,
        Warning,
        Error
    }
}
