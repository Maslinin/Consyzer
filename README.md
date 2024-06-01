[![Build Status](https://github.com/Maslinin/Consyzer/workflows/Build/badge.svg)](https://github.com/Maslinin/Consyzer/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![GitHub license](https://badgen.net/github/license/Maslinin/Consyzer)](https://github.com/Maslinin/Consyzer/blob/master/LICENSE)

## Overview
**Consyzer** is a utility designed to prevent consistency problems of CIL modules when calling external methods implemented in unmanaged Dynamic-Link Libraries (DLLs).

Imagine a scenario where the source code of the application contains calls to external methods implemented in unmanaged Dynamic-Link Libraries.
In the source code of the a module, such calls are described by the **DllImport** attribute and stored in the metadata of the application after its assembly.

The key feature of the **DllImport** attribute is
that the code of the method called from an unmanaged DLL is not directly linked to the source code of a CIL module; 
instead, the metadata of the module stores information about this method, 
including a reference to the expected location of the unmanaged DLL containing the implementation of the method in the system.

```
//In this case, "kernel32.dll" is a reference to the unmanaged DLL containing the definition of the HelloWorld method:
[DllImport("kernel32.dll")]
static extern void HelloWorld();
```

The application works correctly, without violating the integrity and security of the system, if all the unmanaged DLLs are in the locations described in the metadata;
however, if at least one of the DLLs is missing, the application will not only crash, but may also lead to a security breach of the entire system.

Consyzer was developed to prevent such incidents.

## How does it work?
1. Consyzer selects files for analysis according to the directory and the search pattern specified for analysis;
2. Consyzer discards files that are not ECMA-355 metadata assemblies;
3. Consyzer analyzes the modules selected for analysis for methods that are implemented externally;
4. Consyzer logs detailed information about each detected methods that are implemented externally, up to its signature;
5. Consyzer returns the specific analysis code to the operating system in the end, which also allows individual processing of each of the analysis incidents in accordance with your requirements.

> Consyzer logs each of the above steps.

## What information does Consyzer log about the external method?
If Consyzer finds a method that is implemented externally, it logs the following information about it:
1. Method name;
2. The signature of the method;
3. The expected location of the unmanaged DLL containing the definition of the method in the system;
4. Arguments of DllImport attribute.

## Return Codes
> 0 - all unmanaged DLL components used in the analyzed CIL modules exist in the analyzed directory;         
> 1 - one or more unmanaged DLL components used in the analyzed CIL modules exist on the absolute path specified in the metadata;          
> 2 - one or more unmanaged DLL components used in the analyzed CIL modules exist on the relative path specified in the metadata;      
> 3 - one or more unmanaged DLL components used in the analyzed CIL modules exist in the system folder specified in the metadata;        
> 4 - one or more unmanaged DLL components used in the analyzed CIL modules do not exist on the path specified in the metadata.          

> Note that only the last of the return codes indicates a violation of the consistency of the CIL module, while other codes report the location of unmanaged DLL components that is different from the analyzed directory.

> If the location of one of the unmanaged DLL components corresponds to the return code 2 and the other to the return code 3, the utility will return the largest of the codes.

## How to run?
The utility is run from the CLI. Two required arguments must be passed to the utility as input:
1. A directory containing CIL modules for analysis;
2. Extensions of CIL modules to be analyzed.

An example of running Consyzer from the CLI:
```
C:\Consyzer.exe --AnalysisDirectory C:\analysisDirectory --SearchPattern "output.exe, *.dll"
```

## Analysis of multiple projects in the solution
You can use the *PowerShell* script located on ```DevOps\SolutionAnalyzer.ps1``` to analyze the output artifacts of all projects in the solution.
The script can also be used in the **CI/CD pipeline**.