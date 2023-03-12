param(
	[Parameter(Mandatory=$true, HelpMessage="Path to Consyzer executable.")]
	[ValidateScript({Test-Path $_ -PathType Leaf})]
	[String]$pathToConsyzer,
	
	[Parameter(Mandatory=$true, HelpMessage="Path to the solution for analysis.")]
	[ValidateScript({Test-Path $_ -PathType Container})]
	[String]$solutionForAnalysis,
	
	[Parameter(HelpMessage={ "File extensions to scan for. Default is $fileExtensions." })]
	[String]$fileExtensions = ".dll",
	
	[Parameter(HelpMessage={ "Build configuration to use. Default is $buildConfiguration." })]
    [String]$buildConfiguration = "Debug"
)

$defaultSearchParams = @{
	Include = "*.dll", "*.exe"
	Recurse = $true
	Force = $true
}

$pathsToAnalysisFiles = Get-ChildItem -Path . @defaultSearchParams |
	Where-Object { $_.FullName -match ".*\\$buildConfiguration(\\|$)" -and $_.DirectoryName -match "bin" }

if($pathsToAnalysisFiles.Length -eq 0) {
	Write-Warning "No binary files were found for analysis."
	Exit 0
}

#Get output project directories for analysis and then select unique project directories for analysis.
$analysisFolders = $pathsToAnalysisFiles | ForEach-Object { $_.DirectoryName } | Select-Object -Unique

# Scan project artifacts for consistency
$exitCodeMessages = @{
	0 = "Successfully: No consistency problems found."
	1 = "Warning: One or more DLL components used in the project are on an absolute path."
	2 = "Warning: One or more DLL components used in the project are on a relative path."
	3 = "Warning: One or more DLL components used in the project are located on the system path."
	4 = "Error: One or more components used in the DLL project do not found in the expected path."
}

$finalExitCode = -1
$messageBuilder = New-Object System.Text.StringBuilder
foreach ($folder in $analysisFolders) {
	& $pathToConsyzer $folder $fileExtensions
	if ( $LASTEXITCODE -ge $finalExitCode ) {
		$finalExitCode = $LASTEXITCODE
	}
	
	$messageBuilder.AppendLine("$($exitCodeMessages[$LASTEXITCODE]) | $folder")
	Write-Output "Consyzer run exit code: $LASTEXITCODE`n"
}
Write-Output $messageBuilder.ToString()

Exit $finalExitCode
