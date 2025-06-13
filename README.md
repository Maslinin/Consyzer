[![Build Status](https://github.com/Maslinin/Consyzer/workflows/Build/badge.svg)](https://github.com/Maslinin/Consyzer/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Maslinin_Consyzer&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Maslinin_Consyzer) [![GitHub license](https://badgen.net/github/license/Maslinin/Consyzer)](https://github.com/Maslinin/Consyzer/blob/master/LICENSE)

## Overview
**Consyzer** is a CLI utility designed to prevent CIL module consistency issues when using P/Invoke mechanisms to call methods implemented outside the managed CLR environment.

## Why?
In CIL application development, it is not uncommon to need access to methods implemented outside the managed .NET ecosystem. In the source code of a CIL module, such calls are described using the **DllImport** or **LibraryImport** attributes and are stored in the module's metadata after compilation, indicating which unmanaged (native) library should be loaded at runtime and which function should be called from it.

A key feature of such calls is that the function code from the unmanaged library is not linked directly with the CIL module's code;
instead, the module's metadata stores information about the function being called, including a reference to the expected location of the unmanaged library containing the implementation of that function on the system.

```csharp
// In this example, "foo.dll" is a reference to an unmanaged library containing the implementation of the HelloWorld function:
[DllImport("foo.dll")]
static extern void HelloWorld();

// or

[LibraryImport("foo.dll")]
static partial void HelloWorld();
```

The application functions correctly without compromising system integrity and security as long as all unmanaged libraries are present at the locations described in the metadata;
however, if even one of the libraries is missing, the application will not only crash but may also pose a security risk.

Consyzer was created to ensure that such situations do not come as a surprise.

## How does it works?
1. Consyzer selects files for analysis based on the specified directory and search pattern;  
2. Consyzer logs and excludes from analysis files that are not ECMA-355 assemblies;  
3. Consyzer logs detailed information about each ECMA-355 assembly found, including its hash;  
4. Consyzer analyzes the found ECMA-355 assemblies for P/Invoke calls;  
5. Consyzer logs detailed information about each discovered method, including its signature;  
6. Consyzer returns an exit code indicating the specific result of the analysis, which allows you to handle each analysis incident individually according to your needs.

> Consyzer logs each of the steps described above.

## What information does Consyzer log about found P/Invoke calls?
If Consyzer finds P/Invoke calls in an assembly, it additionally logs the following information:
1. Method name;  
2. Method signature;  
3. Expected name or location of the unmanaged library containing the function implementation on the system;  
4. Flags of the `DllImport` or `LibraryImport` attribute.

Example log containing information about P/Invoke methods:
```
[0] File: Foo.dll — Found: 1
	[0]
		Method Signature: 'Int32 static .NativeMethods.HelloWorld()'
		DLL Location: 'libfoobar.so'
		DLL Import Flags: 'CallingConventionCDecl'
[1] File: Bar.dll — Found: 2
	[0]
		Method Signature: 'Void static Foo.Tasks.HelloThere(Single)'
		DLL Location: 'phantom.dll'
		DLL Import Flags: 'CallingConventionStdCall'
	[1]
		Method Signature: 'Void static Foo.Tasks.WhatAFunc(Char[], Int32)'
		DLL Location: 'phantom.dll'
		DLL Import Flags: 'CallingConventionStdCall'
```

## Exit codes
Consyzer returns a specific exit code depending on where the native libraries specified in the P/Invoke attributes were or were not found:

| Code | Meaning                                                                          |
|------|----------------------------------------------------------------------------------|
| -5   | None of the found files were valid for analysis                                  |
| -4   | No files matching the search pattern were found in the analysis directory        |
| -3   | No file search pattern was specified                                             |
| -2   | No directory was specified for analysis                                          |
| -1   | An unexpected error occurred during analysis                                     |
| 0    | All libraries were found in the analysis directory                               |
| 1    | One or more libraries were found via the **PATH** environment variable           |
| 2    | One or more libraries were found in the system directory                         |
| 3    | One or more libraries were found via an absolute path                            |
| 4    | One or more libraries were found via a relative path                             |
| 5    | One or more libraries are missing from the system                                |

> Note that only the last code indicates a module consistency violation.  
> If libraries are found in different locations, the highest of the corresponding codes is returned.  
> Negative codes indicate configuration or runtime errors.

## How do I run it?
**Consyzer** is launched from the command line (CLI) and requires two mandatory parameters:
1. A directory containing the CIL modules to analyze;
2. A file search pattern specifying which CIL modules should be analyzed.

### General usage pattern
```
Consyzer.exe --AnalysisDirectory <path_to_directory> --SearchPattern <search_pattern>
```

### Example
```
Consyzer.exe --AnalysisDirectory C:\Modules --SearchPattern "output.exe, *.dll"
```

## Analyzing multiple projects in a solution
You can use [this](https://github.com/Maslinin/Consyzer/blob/master/DevOps/SolutionAnalyzer.ps1) PowerShell script to analyze the output artifacts of all projects in a solution.  
This script can also be used in a **CI/CD pipeline**.
