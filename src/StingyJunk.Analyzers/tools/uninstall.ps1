param($installPath, $toolsPath, $package, $project)

Write-Host "Using tools path $toolsPath"

$analyzersPaths = Join-Path (Join-Path (Split-Path -Path $toolsPath -Parent) "lib\portable45-net45+win8" ) * -Resolve

Write-Host "detected analyzer paths... $analyzersPaths"

foreach($analyzersPath in $analyzersPaths)
{
    # Uninstall the language agnostic analyzers.
	Write-Host "using path $analyzersPath" 

    if (Test-Path $analyzersPath)
    {
        foreach ($analyzerFilePath in Get-ChildItem $analyzersPath -Filter *.dll)
        {
			Write-Host "using file path $analyzerFilePath" 

            if($project.Object.AnalyzerReferences)
            {
                $project.Object.AnalyzerReferences.Remove($analyzerFilePath.FullName)
            }
        }
    }
}
