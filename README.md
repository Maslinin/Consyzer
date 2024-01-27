[![Build Status](https://github.com/Maslinin/Consyzer/workflows/Build/badge.svg)](https://github.com/Maslinin/Consyzer/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![GitHub license](https://badgen.net/github/license/Maslinin/Consyzer)](https://github.com/Maslinin/Consyzer/blob/master/LICENSE)

# Description
**Consyzer** is a utility designed to solve consistency problems of CIL assemblies deployed in the system.

## What is the purpose?
Imagine that in the source code of some application there are calls to functions located in third-party unmanaged dynamic link libraries (DLL). 
In the source code of the CIL module, such calls are described using the **DllImport** instructions and stored in its metadata after the application is built.       
The key feature of the **DllImport** attribute is that the function called from an unmanaged DLL is not directly linked to the source code of the CLI module; 
instead, the module metadata stores information about the imported function, including a reference to the expected location of the unmanaged DLL on the system in which this function is defined.
```
//In this context "kernel32.dll" is the relative path to the DLL containing the definition of the HelloWorld function:
[DllImport("kernel32.dll")]
static extern void HelloWorld();
```

The application works correctly, without violating the integrity and security of the system, if all unmanaged DLLs are on the intended location according to the metadata; 
but if at least one of the Dll is missing, the application will not only crash, but may also lead to a security breach of the entire system.

Consyzer was designed specifically to prevent such incidents.

## Benefits of Consyzer
1. Consyzer is GUI independent and easy to run from *CLI*; 
2. Consyzer can be easily integrated into *CI/CD pipeline*;
3. Consyzer logs detailed information about each imported function, up to its signature;
4. Consyzer returns a specific analysis code to the operating system, which allows you to respond individually to each of the analysis incidents.

## What information does the Consyzer log about the imported function?
> 0 - all the DLL files used by the application are located in the analyzed directory along with the application artifacts;           
> 1 - one or more DLLs used in the CIL module of the application are on an absolute path;         
> 2 - one or more DLLs used in the CIL module of the application are on a relative path;        
> 3 - one or more DLLs used in the CIL module of the application are located in the system folder;         
> 4 - one or more DLLs used in the CIL module of the application are missing in the path specified in the module metadata.          

> Note that only the last code returned indicates an application consistency.

## Run
Two parameters are passed to the utility as input:
1. Directory containing CIL files for analysis. These may be the output artifacts of the project;
2. File extensions to be analyzed.

The common template for running *Consyzer* from the CLI looks like this:
```
pathToConsyzer pathToFolderWithApplicationArtifacts extensionsOfAnalyzedFiles
```

Example of running *Consyzer* from the CLI:
```
C:\Consyzer.exe --AnalysisDirectory C:\folderToBeAnalyzed --SearchPattern "*.exe, *.dll"
```

## Analysis of multiple projects
You can use the *PowerShell* script located on the path ```DevOps\SolutionAnalyzer.ps1```  to analyze the output artifacts of all projects in a solution using *Consyzer*.
It can also be useful in *CI/CD pipeline*.

Read more in ```DevOps\README.md```.
