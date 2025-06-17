# DevOps solutions

## Analyze all projects in a solution

Use the PowerShell script `SolutionAnalyzer.ps1` to run **Consyzer** on all compiled projects in a solution.  
This is especially useful for use in **CI/CD pipelines** after a build.

### Usage

```powershell
.\SolutionScanner.ps1 `
  -pathToConsyzer "C:\Tools\Consyzer.exe" `
  -solutionForAnalysis "C:\Path\To\Solution" `
  -searchPattern "*.exe, *.dll" `
  -buildConfiguration "Release"
```

Arguments:
- `-pathToConsyzer` – Full path to the **Consyzer** executable.
- `-solutionForAnalysis` – Path to the folder containing the solution or its build output.
- `-searchPattern` – Comma-separated list of file patterns to scan (e.g. `"*.exe, *.dll"`). Default: `"*.exe, *.dll"`
- `-buildConfiguration` – Build configuration folder to target (default: `"Release"`).

### Azure Pipelines

To use this in Azure Pipelines:

```yaml
- task: PowerShell@2
  inputs:
    targetType: 'filePath'
    filePath: '$(Build.SourcesDirectory)\DevOps\SolutionScanner.ps1'
    arguments: >
      -pathToConsyzer "$(Build.BinariesDirectory)\Consyzer.exe"
      -solutionForAnalysis "$(Build.SourcesDirectory)"
      -searchPattern "*.exe, *.dll"
      -buildConfiguration "Release"
  displayName: 'Consyzer Solution Analysis'
```
