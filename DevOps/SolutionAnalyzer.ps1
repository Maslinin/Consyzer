param(
	[Parameter(Mandatory=$true, HelpMessage="Path to Consyzer executable.")]
	[ValidateScript({Test-Path $_ -PathType Leaf})]
	[String]$pathToConsyzer,
	
	[Parameter(Mandatory=$true, HelpMessage="Path to the solution for analysis.")]
	[ValidateScript({Test-Path $_ -PathType Container})]
	[String]$solutionForAnalysis,
	
	[Parameter(HelpMessage={ "File extensions to scan for. Default is $searchPattern." })]
	[String]$searchPattern = "*.exe, *.dll",
	
	[Parameter(HelpMessage={ "Build configuration to use. Default is $buildConfiguration." })]
    [String]$buildConfiguration = "Release"
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

$finalExitCode = -1
$messageBuilder = New-Object System.Text.StringBuilder
foreach ($folder in $analysisFolders) {
	& $pathToConsyzer --AnalysisDirectory $folder --SearchPattern $searchPattern
	if ( $LASTEXITCODE -ge $finalExitCode ) {
		$finalExitCode = $LASTEXITCODE
	}
	
	# Scan project artifacts for consistency
	$exitCodeMessages = @{
		0 = "Successfully: $folder. Consistency problems have not been found."
		1 = "Warning: $folder. One or more DLL components used in the project are on the absolute path."
		2 = "Warning: $folder. One or more DLL components used in the project are on the relative path."
		3 = "Warning: $folder. One or more DLL components used in the project are in the system directory."
		4 = "Error: $folder. One or more DLL components used in the project have not been found on the expected path."
	}
	
	$messageBuilder.AppendLine($exitCodeMessages[$LASTEXITCODE])
	Write-Output "Consyzer run exit code: $LASTEXITCODE`n"
}
Write-Output $messageBuilder.ToString()

Exit $finalExitCode