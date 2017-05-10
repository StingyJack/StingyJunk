#post build event
#NuGet  pack "$(ProjectPath)" -NoPackageAnalysis  -OutputDirectory "c:\temp\nuget"

NuGet  pack "StingyJunk.Analyzers.csproj" -NoPackageAnalysis  -OutputDirectory "c:\temp\nuget"