param($installPath, $toolsPath, $package, $project)

Write-Host "Using tools path $toolsPath"

$analyzersPaths = Join-Path (Join-Path (Split-Path -Path $toolsPath -Parent) "lib\portable45-net45+win8" ) * -Resolve

Write-Host "detected analyzer paths... $analyzersPaths"

foreach($analyzersPath in $analyzersPaths)
{

	Write-Host "using path $analyzersPath" 

    # Install the language agnostic analyzers.
    if (Test-Path $analyzersPath)
    {
        foreach ($analyzerFilePath in Get-ChildItem $analyzersPath -Filter *.dll)
        {
			Write-Host "using file path $analyzerFilePath" 

            if($project.Object.AnalyzerReferences)
            {
                $project.Object.AnalyzerReferences.Add($analyzerFilePath.FullName)
            }
        }
    }
}


$item = $project.ProjectItems.Item("ForbiddenReferences.xml")
$item.Properties.Item("BuildAction").Value = 4