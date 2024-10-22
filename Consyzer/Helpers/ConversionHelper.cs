using Consyzer.Checkers.Models;
using Consyzer.Extractors;
using Consyzer.Extractors.Models;

namespace Consyzer.Helpers;

internal static class ConversionHelper
{
    public static IEnumerable<FileExistenceInfo> ToFileExistenceInfos(this IDictionary<string, FileExistenceStatus> fileExistenceStatuses)
    {
        return fileExistenceStatuses.Select(s => new FileExistenceInfo { FilePath = s.Key, ExistenceStatus = s.Value});
    }

    public static IEnumerable<IEcmaImportedMethodExtractor> ToImportedMethodExtractors(this IEnumerable<FileInfo> metadataAssemblyFiles)
    {
        return metadataAssemblyFiles.Select(f => new MetadataImportedMethodExtractor(f));
    }

    public static IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> ToImportedMethodInfos(this IEnumerable<IEcmaImportedMethodExtractor> importedMethodExtractors)
    {
        return importedMethodExtractors.ToDictionary(k => k.MetadataAssembly, v => v.GetImportedMethodInfos());
    }

    public static IEnumerable<string> ToDllLocations(this IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> importedMethodInfos)
    {
        return importedMethodInfos.SelectMany(m => m.Value.Select(i => i.DllLocation)).Distinct();
    }
}