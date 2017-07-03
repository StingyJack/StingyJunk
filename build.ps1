cls
$ErrorActionPreference="Continue"

$loc = $PSScriptRoot
$nugetOutFolder = "$PSScriptRoot\nugetpackages"
$slnFilePath = "$PSScriptRoot\src\StingyJunk.sln"
$sjCompPath = "$PSScriptRoot\src\StingyJunk.Compilation"
$sjCsProjFilePath = "$sjCompPath\StingyJunk.Compilation.csproj"
$sjCompNuspec = "$sjCompPath\StingyJunk.Compilation.nuspec"

$nugetTarget = $sjCompNuspec
$nugetTargets = @("StingyJunk.Compilation", "StingyJunk.IO", "StingyJunk.Console")

if ((Test-Path $nugetOutFolder) -eq $false)
{
	New-Item -Path $nugetOutFolder -ItemType Directory | Out-Null
}

#nuget.exe restore

#dont feel like being smart about this.
Write-Host "Cleaning Debug outputs"
E:\vs2017.2\MSBuild\15.0\Bin\MSBuild.exe .\src\StingyJunk.sln /toolsversion:15.0 `
	/p:Configuration=Debug /p:Platform="Any CPU" `
	/t:Clean /verbosity:m

Write-Host "Cleaning Release outputs"
E:\vs2017.2\MSBuild\15.0\Bin\MSBuild.exe .\src\StingyJunk.sln /toolsversion:15.0 `
	/p:Configuration=Release /p:Platform="Any CPU" `
	/t:Clean /verbosity:m

Write-Host "Running Restore and Build for $slnFilePath"
E:\vs2017.2\MSBuild\15.0\Bin\MSBuild.exe $slnFilePath   /toolsversion:15.0 `
	/p:Configuration=Release `
	/p:Platform="Any CPU" `
	/t:Restore,Build /verbosity:m `
	/p:NuspecFile=$sjCompNuspec `
	/p:NuspecBasePath=$sjCompPath `
	/p:NuspecProperties=\"OutputDirectory=$nugetOutFolder\"    
	


foreach ($nugetTarget in $nugetTargets)
{
	Write-Host "Running nuget pack for $nugetTarget"
	$projectFolder= "$PSScriptRoot\src\$nugetTarget"
	$projFilePath = "$projectFolder.csproj"
	$nuspecPath= "$projectFolder\$nugetTarget.nuspec"

	Write-Host "Using nuspec path $nuspecPath"

	nuget.exe pack $nuspecPath `
		-Properties "Configuration=Release;Platform=`"Any CPU`""   `
		-BasePath $projectFolder  `
		-OutputDirectory $nugetOutFolder `
		-Symbols `
		-Verbosity Detailed `
		#-MSBuildPath "E:\vs2017.2\MSBuild\15.0\Bin\" `
		


}

#.\.nuget\nuget.exe pack $nugetTarget `
#	-Properties "Configuration=Release;Platform=`"Any CPU`""   `
#	-BasePath $sjCompPath  `
#	-OutputDirectory $nugetOutFolder `
#	-Verbosity Detailed `
#	-MSBuildPath "E:\vs2017.2\MSBuild\15.0\Bin\" `
#	-Symbols


Set-Location $loc