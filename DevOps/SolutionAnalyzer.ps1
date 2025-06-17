param(
    [Parameter(Mandatory = $true, HelpMessage = "Path to Consyzer executable.")]
    [ValidateScript({ Test-Path $_ -PathType Leaf })]
    [string]$pathToConsyzer,

    [Parameter(Mandatory = $true, HelpMessage = "Path to the solution for analysis.")]
    [ValidateScript({ Test-Path $_ -PathType Container })]
    [string]$solutionForAnalysis,

    [Parameter(HelpMessage = "File extensions to scan for. Default is '*.exe,*.dll'.")]
    [string]$searchPattern = "*.exe,*.dll",

    [Parameter(HelpMessage = "Build configuration to use. Default is 'Release'.")]
    [string]$buildConfiguration = "Release"
)

Set-Location $solutionForAnalysis

$defaultSearchParams = @{
    Include = "*.dll", "*.exe"
    Recurse = $true
    Force = $true
}

$pathsToAnalysisFiles = Get-ChildItem -Path . @defaultSearchParams |
    Where-Object {
        $_.FullName -match ".*\\$buildConfiguration(\\|$)" -and $_.DirectoryName -match "bin"
    }

if ($pathsToAnalysisFiles.Length -eq 0) {
    Write-Warning "No binary files were found for analysis."
    Exit 0
}

$analysisFolders = $pathsToAnalysisFiles |
    ForEach-Object { $_.DirectoryName } |
    Select-Object -Unique

$finalExitCode = -1
$messageBuilder = New-Object System.Text.StringBuilder

foreach ($folder in $analysisFolders) {
    & $pathToConsyzer --AnalysisDirectory $folder --SearchPattern $searchPattern

    if ($LASTEXITCODE -ge $finalExitCode) {
        $finalExitCode = $LASTEXITCODE
    }

    $message = switch ($LASTEXITCODE) {
        -4 { "Error: $folder → none of the files were valid for analysis." }
        -3 { "Error: $folder → no files matched the search pattern." }
        -2 { "Error: $folder → no search pattern was specified." }
        -1 { "Error: $folder → no analysis directory was specified." }
         0 { "Success: $folder → all libraries found in the analysis directory." }
         1 { "Warning: $folder → one or more libraries were found via PATH." }
         2 { "Warning: $folder → one or more libraries found in system directories." }
         3 { "Warning: $folder → one or more libraries found via absolute path." }
         4 { "Warning: $folder → one or more libraries found via relative path." }
         5 { "Error: $folder → one or more libraries are missing from the system." }
        default { "Unknown exit code ($LASTEXITCODE) for $folder." }
    }

    $messageBuilder.AppendLine($message)
    Write-Output "`nConsyzer exit code: $LASTEXITCODE`n"
}

Write-Output $messageBuilder.ToString()
Exit $finalExitCode
