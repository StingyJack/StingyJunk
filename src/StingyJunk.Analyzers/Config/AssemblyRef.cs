namespace StingyJunk.Analyzers.Config
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;

    [DataContract]
    public class AssemblyRef
    {
        [DataMember]
        public string NameMatch { get; set; }
        [DataMember]
        public string VersionGreaterThan { get; set; }

        private Version _Version;
        private Version ParsedVersion
        {
            get
            {
                if (_Version != null) { return _Version; }
                if (Version.TryParse(VersionGreaterThan, out _Version) == false)
                {
                    throw new ArgumentException($"cant parse version {VersionGreaterThan} for comparison");    
                }
                return _Version;
            }
        }

        private enum EvalType
        {
            None,
            NameMatch,
            NameAndVersionGreater
        }

        public bool IsForbidden(AssemblyIdentity candidate)
        {
            var evalType = GetEvalType();
            switch (evalType)
            {
                case EvalType.None:
                    return false;
                case EvalType.NameMatch:
                    return DoesNameMatch(candidate);
                case EvalType.NameAndVersionGreater:
                    return DoesNameMatch(candidate) && IsVersionGreater(candidate);
            }

            return false;
        }

        private bool IsVersionGreater(AssemblyIdentity candidate)
        {
            /* Per MSDN
             * Less than zero - The current Version object is a version before version.
             * Zero - The current Version object is the same version as version.
             * Greater than zero - The current Version object is a version subsequent to version.
             *                      -or-
             *                      candidate is null.
             */
            var result = ParsedVersion.CompareTo(candidate.Version);
            
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        private EvalType GetEvalType()
        {
            if (string.IsNullOrWhiteSpace(NameMatch))
            {
                return EvalType.None;
            }

            if (string.IsNullOrWhiteSpace(VersionGreaterThan))
            {
                return EvalType.NameMatch;
            }
            return EvalType.NameAndVersionGreater;
        }

        private bool DoesNameMatch(AssemblyIdentity candidate)
        {
            if (Regex.IsMatch(candidate.Name, NameMatch))
            {
                return true;
            }
            return false;
        }
    }
}