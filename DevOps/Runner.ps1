param(
    [Parameter()]
    [String]$pathToConsyzer,
    [String]$solutionForAnalysis,
    [String]$fileExtensions,
    [String]$buildConfiguration
)

switch ($null) {
    $pathToConsyzer { 
        Write-Host "Missing pathToConsyzer parameter. Script will exit."
        Exit -1
    }
    $solutionForAnalysis {
        Write-Host "Missing solutionForAnalysis parameter. Script will exit."
        Exit -1
    }
    $fileExtensions {
        Write-Host "Missing fileExtensions parameter. Script will use default value."
        $fileExtensions = ".dll"
    }
    $buildConfiguration {
        Write-Host "Missing buildConfiguration parameter. Script will use default value."
        $buildConfiguration = "Debug"
    }
}


#Project artifacts almost always contain DLL files; therefore, in order to avoid duplication of found paths containing artifacts, we are looking only for those paths that contain DLL files.
#If you are firmly convinced that your application does not contain a DLL, replace "*.dll" with "*.exe" in the next line.
$paths = $( Get-ChildItem -Path . -Include "*.dll"  -Recurse -Force )

#Search for artifacts for analysis
$pathsToAnalysisFiles = New-Object System.Collections.Generic.List[System.IO.FileInfo]
foreach($file in $paths) {
  if($file -match "bin" -and "$buildConfiguration" -and $file -notmatch "runtimes") {
	$pathsToAnalysisFiles.Add($file)
  }
}

if($pathsToAnalysisFiles.length -eq 0) {
  Write-Host "No binary files were found for analysis."
  Exit 0
}

#Get output project directories for analysis.
$AnalysisFolders = New-Object System.Collections.Generic.List[System.String]
foreach($file in $pathsToAnalysisFiles) {
  $folder = $($file.DirectoryName)
  $AnalysisFolders.Add($folder)
}
$AnalysisFolders = $AnalysisFolders | Select-Object -Unique

#Scanning of projects artifacts.
$finalExitCode = -1
$AnalysisStatuses = New-Object System.Collections.Generic.List[System.String]
foreach($folder in $AnalysisFolders) {
  & $pathToConsyzer $folder $fileExtensions
  if ( $LastExitCode -eq $exitcode ) {
	Write-Host "Error|$folder-> Consyzer could not analyze the files because an internal error occurred. Make sure that the arguments were passed correctly."
  }
  
  if ( $LastExitCode -ge $exitcode ) {
	$finalExitCode = $LastExitCode
  }
  Write-Host "Consyzer run exit code: " $LastExitCode `n
  
  #Determining the status of scanning project artifacts
  switch( $LastExitCode )
  {
      0 { $Event = "Successfully|$folder-> No consistency problems were found." }
      1 { $Event = "Warning|$folder->  One or more DLL components used in the project are on an absolute path." }
      2 { $Event = "Warning|$folder->  One or more DLL components used in the project are on a relative path." }
      3 { $Event = "Warning|$folder->  One or more DLL components used in the project are located on the system path." }
      4 { $Event = "Error|$folder->  One or more components used in the DLL project were not found on the expected path." }
  }
  $AnalysisStatuses.Add($Event)
}

foreach($status in $AnalysisStatuses) { Write-Host $status }
Exit $finalExitCode