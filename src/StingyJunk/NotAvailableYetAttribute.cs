namespace StingyJunk
{
    using System;

    /// <summary>
    ///     Stick it on a class, member, enum value, etc to note that its not yet complete. 
    /// </summary>
    /// <remarks>
    ///     There is a difference between things being visible to end users and things being 
    /// visible to other developers working on a project. If you need to commit changes to 
    /// source control and they are still in progress (but not user visible - by UX or execution 
    /// path), this attribute helps mark that scenario.
    /// </remarks>
    [Obsolete("The member or class this is applied to is planned or in progress but is not yet available for use.")]
    public class NotAvailableYetAttribute : Attribute
    {
    }
}