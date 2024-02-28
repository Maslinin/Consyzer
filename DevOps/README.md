# DevOps solutions

## Analysis of multiple projects in the solution
You can use the *PowerShell* script ```SolutionScanner.ps1``` to analyze the output artifacts of the entire solution by using Consyzer.
In other words, this script allows you to analyze the artifacts of all projects in the solution.
It can be useful in the *CI/CD pipeline*.

To run the script, use the following command:       
```SolutionScanner.ps1 C:\Consyzer.exe C:\SolutionToBeAnalyzed ".exe, .dll" Release```, where        
1) *SolutionScanner.ps1* - name of the Powershell script (or its path),          
2) *C:\Consyzer.exe* - path to Consyzer utility,       
3) *C:\SolutionToBeAnalyzed* - path to the analyzed solution,        
4) *".exe, .dll"* - files search pattern to be analyzed,        
5) *Release* - solution build configuration.       

You can use the following task for analysis if you are using Azure Pipelines:        
```
- task: PowerShell@2
      inputs:
        targetType: 'filePath'
        filePath: '$(Build.SourcesDirectory)\SolutionAnalyzer.ps1'
        arguments: >
          -pathToConsyzer '$PathToConsyzer'
          -solutionForAnalysis '$(Build.SourcesDirectory)'
          -fileExtensions '"*.exe, *.dll"'
          -buildConfiguration '$YourBuildConfiguration'
      displayName: 'Consyzer Analysis'
```