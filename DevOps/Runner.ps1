[CmdletBinding()]
param(
    [Parameter(Mandatory=$true, HelpMessage="Path to Consyzer executable.")]
	[ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]$pathToConsyzer,
    [Parameter(Mandatory=$true, HelpMessage="Path to the solution for analysis.")]
	[ValidateScript({Test-Path $_ -PathType Container})]
    [String]$solutionForAnalysis,
    [String]$fileExtensions = ".dll",
    [String]$buildConfiguration = "Debug"
)

if (-not ($fileExtensions.StartsWith("."))) {
    Write-Warning "Invalid file extension format. The extension should start with a dot. The default value is set to `$fileExtensions`."
    $fileExtensions = "$fileExtensions"
}
if (-not $buildConfiguration) {
    Write-Warning "Missing buildConfiguration parameter. Script will use the default $buildConfiguration value."
    $buildConfiguration = "Debug"
}

#Project artifacts almost always contain DLL files; therefore, in order to avoid duplication of found paths containing artifacts, we are looking only for those paths that contain DLL files.
$defaultParams = @{
    Filter = "*.dll"
    Recurse = $true
    Force = $true
}

$pathsToAnalysisFiles = Get-ChildItem -Path . @defaultParams |
    Where-Object { $_.FullName -match ".*\\$stopFolderName(\\|$)" -and $_.DirectoryName -match "bin" }

if($pathsToAnalysisFiles.Length -eq 0) {
    Write-Warning "No binary files were found for analysis."
    Exit 0
}

#Get output project directories for analysis and then select unique project directories for analysis.
$analysisFolders = $pathsToAnalysisFiles.DirectoryName | Select-Object -Unique

# Scan project artifacts for consistency
$finalExitCode = -1
$analysisStatuses = foreach ($folder in $analysisFolders) {
$result = & $pathToConsyzer $folder $fileExtensions
    $exitCodeMessages = @{
        0 = "Successfully|$folder -> No consistency problems were found."
        1 = "Warning|$folder -> One or more DLL components used in the project are on an absolute path."
        2 = "Warning|$folder -> One or more DLL components used in the project are on a relative path."
        3 = "Warning|$folder -> One or more DLL components used in the project are located on the system path."
        4 = "Error|$folder -> One or more components used in the DLL project were not found on the expected path."
    }
    $analysisStatus = $exitCodeMessages[$result]
    Write-Output "Consyzer run exit code: $result`n"
    $analysisStatus
}

$analysisStatuses | Write-Output
Exit $finalExitCode