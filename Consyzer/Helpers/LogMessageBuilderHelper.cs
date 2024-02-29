using Consyzer.Cryptography;
using Consyzer.Checkers.Models;
using Consyzer.Extractors.Models;
using Consyzer.Extractors.Models.Extensions;
using Consyzer.Options;

namespace Consyzer.Helpers;

internal static class LogMessageBuilderHelper
{
    public static string BuildAnalysisParamsLog(CommandLineOptions arguments)
    {
        string analysisDirectory = $"The specified directory for analysis: '{arguments.AnalysisDirectory}'.";
        string searchPattern = $"The specified file search pattern for analysis: {arguments.SearchPattern}.";

        return $"{analysisDirectory}{Environment.NewLine}{searchPattern}";
    }

    public static string BuildBaseFileInfoLog(IEnumerable<FileInfo> fileInfos)
    {
        return string.Join(Environment.NewLine, fileInfos.Select((f, i) =>
        {
            string fileInfo = $"[{i}]File: '{f.Name}':";
            string creationTime = $"\tCreation Time: '{f.CreationTime}'";

            return $"{fileInfo}{Environment.NewLine}{creationTime}";
        }));
    }

    public static string BuildBaseAndHashFileInfoLog(IEnumerable<FileInfo> files, IFileHasher hasher)
    {
        return string.Join(Environment.NewLine, files.Select((f, i) =>
        {
            string fileInfo = $"[{i}]File: '{f.Name}':";
            string creationTime = $"\tCreation Time: '{f.CreationTime}'";
            string hash = $"\tSHA256 Hash Sum: '{hasher.CalculateHash(f)}'.";

            return $"{fileInfo}{Environment.NewLine}{creationTime}, {Environment.NewLine}{hash}";
        }));
    }

    public static string GetImportedMethodsInfoForEachFileLog(IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> fileInfoImportedMethodPairs)
    {
        return string.Join(Environment.NewLine, fileInfoImportedMethodPairs.Select((pair, index) =>
        {
            string file = $"[{index}]File '{pair.Key}': {Environment.NewLine}";

            if (pair.Value.Any())
            {
                string methodInfoText = BuildImportedMethodsInfoLog(pair.Value);
                return $"{file}{methodInfoText}";
            }

            return $"{file}\tThe CIL module does not contain external functions from any unmanaged assemblies.";
        }));
    }

    public static string BuildImportedMethodsInfoLog(IEnumerable<ImportedMethodInfo> importedMethods)
    {
        return string.Join(Environment.NewLine, importedMethods.Select((info, index) =>
        {
            string methodLocation = $"\t[{index}]Method '{info.Signature.GetMethodLocation()}':";
            string methodSignature = $"\t\tMethod Signature: '{info.Signature.GetMethodSignature()}',";
            string dllLocation = $"\t\tDLL Location: '{info.DllLocation}',";
            string dllImportArgs = $"\t\tDllImport Args: '{info.DllImportArgs}'.";

            return $"{methodLocation}{Environment.NewLine}{methodSignature}{Environment.NewLine}{dllLocation}{Environment.NewLine}{dllImportArgs}";
        }));
    }
    
    public static string BuildFileExistenceInfoLog(IEnumerable<FileExistenceInfo> fileExistenceInfos)
    {
        return string.Join(Environment.NewLine, fileExistenceInfos.Select((info, index) =>
        {
            string status = info.ExistenceStatus != FileExistenceStatus.FileDoesNotExist ? $"exists ({info.ExistenceStatus})" : "DOES NOT exist";
            return $"\t[{index}]File '{info.FilePath}' {status}.";
        }));
    }
}