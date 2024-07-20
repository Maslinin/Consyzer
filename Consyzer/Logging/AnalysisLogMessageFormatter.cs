using Consyzer.Checkers.Models;
using Consyzer.Extractors.Models;
using Consyzer.Extractors.Models.Extensions;

namespace Consyzer.Logging;

internal static class AnalysisLogMessageFormatter
{
    public static string GetExternalMethodsInfoForEachFileLog(IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> fileInfoImportedMethodPairs)
    {
        return string.Join(Environment.NewLine, fileInfoImportedMethodPairs.Select((pair, index) =>
        {
            string file = $"[{index}]File '{pair.Key}': {Environment.NewLine}";

            if (pair.Value.Any())
            {
                string methodInfoText = GetExternalMethodsInfoLog(pair.Value);
                return $"{file}{methodInfoText}";
            }

            return $"{file}\tThe CIL module does not contain external methods from any unmanaged assemblies.";
        }));
    }

    public static string GetExternalMethodsInfoLog(IEnumerable<ImportedMethodInfo> importedMethods)
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

    public static string GetFileExistenceInfoLog(IEnumerable<FileExistenceInfo> fileExistenceInfos)
    {
        return string.Join(Environment.NewLine, fileExistenceInfos.Select((info, index) =>
        {
            string status = info.ExistenceStatus != FileExistenceStatus.FileDoesNotExist ? $"exists ({info.ExistenceStatus})" : "DOES NOT exist";
            return $"\t[{index}]File '{info.FilePath}' {status}.";
        }));
    }
}