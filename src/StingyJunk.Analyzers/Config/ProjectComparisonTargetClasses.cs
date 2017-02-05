namespace StingyJunk.Analyzers.Config
{

    /// <remarks>
    ///     This is a copy and "Paste XML as Classes" from a project file
    /// </remarks>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false)]
    public class ProjectComparisonTarget
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Import", typeof(ProjectImport))]
        [System.Xml.Serialization.XmlElementAttribute("ItemGroup", typeof(ProjectItemGroup))]
        [System.Xml.Serialization.XmlElementAttribute("PropertyGroup", typeof(ProjectPropertyGroup))]
        [System.Xml.Serialization.XmlElementAttribute("Target", typeof(ProjectTarget))]
        public object[] Items { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public decimal ToolsVersion { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string DefaultTargets { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectImport
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Project { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroup
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Analyzer")]
        public ProjectItemGroupAnalyzer[] Analyzer { get; set; }

        /// <remarks/>
        public ProjectItemGroupService Service { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("None")]
        public ProjectItemGroupNone[] None { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Compile")]
        public ProjectItemGroupCompile[] Compile { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Reference")]
        public ProjectItemGroupReference[] Reference { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupAnalyzer
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupService
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupNone
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupCompile
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectItemGroupReference
    {
        /// <remarks/>
        public string HintPath { get; set; }

        /// <remarks/>
        public string Private { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Include { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroup
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AppDesignerFolder", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("CodeAnalysisRuleSet", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Configuration", typeof(ProjectPropertyGroupConfiguration))]
        [System.Xml.Serialization.XmlElementAttribute("DebugSymbols", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("DebugType", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("DefineConstants", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("ErrorReport", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("FileAlignment", typeof(ushort))]
        [System.Xml.Serialization.XmlElementAttribute("Optimize", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("OutputPath", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Platform", typeof(ProjectPropertyGroupPlatform))]
        [System.Xml.Serialization.XmlElementAttribute("PlatformTarget", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("PreBuildEvent", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("TargetFrameworkVersion", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("WarningLevel", typeof(byte))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupConfiguration
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectPropertyGroupPlatform
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        AppDesignerFolder,

        /// <remarks/>
        CodeAnalysisRuleSet,

        /// <remarks/>
        Configuration,

        /// <remarks/>
        DebugSymbols,

        /// <remarks/>
        DebugType,

        /// <remarks/>
        DefineConstants,

        /// <remarks/>
        ErrorReport,

        /// <remarks/>
        FileAlignment,

        /// <remarks/>
        Optimize,

        /// <remarks/>
        OutputPath,

        /// <remarks/>
        Platform,

        /// <remarks/>
        PlatformTarget,

        /// <remarks/>
        PreBuildEvent,

        /// <remarks/>
        TargetFrameworkVersion,

        /// <remarks/>
        WarningLevel,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectTarget
    {
        /// <remarks/>
        public ProjectTargetPropertyGroup PropertyGroup { get; set; }

        /// <remarks/>
        public ProjectTargetError Error { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string BeforeTargets { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectTargetPropertyGroup
    {
        /// <remarks/>
        public string ErrorText { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectTargetError
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Condition { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Text { get; set; }
    }


}
