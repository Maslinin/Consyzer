## Analysis of multiple projects
You can use the *PowerShell* script ```Runner.ps1``` to analyze the output artifacts of the entire solution by using *Consyzer*.
In other words, this script allows you to analyze the artifacts of all projects in the solution.
For example, it can be useful in the *CI/CD pipeline*.

To run the script, use the following command:       
```Runner.ps1 C:\Consyzer.exe C:\SolutionToBeAnalyzed ".exe, .dll" Release```, where        
1) *Runner.ps1* - name of the Powershell script (or its path),          
2) *C:\Consyzer.exe* - path to the Consyzer utility,       
3) *C:\SolutionToBeAnalyzed* - path to the analyzed solution,        
4) *".exe, .dll"* - files extensions to be analyzed,        
5) *Release* - solution build configuration.       