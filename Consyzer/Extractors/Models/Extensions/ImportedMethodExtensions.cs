namespace Consyzer.Extractors.Models.Extensions;

internal static class ImportedMethodExtensions
{
    public static IEnumerable<IEcmaImportedMethodExtractor> ToImportedMethodExtractors(this IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Select(f => new MetadataImportedMethodExtractor(f));
    }

    public static IEnumerable<IEnumerable<ImportedMethodInfo>> ToImportedMethodInfos(this IEnumerable<IEcmaImportedMethodExtractor> importedMethodExtractors)
    {
        return importedMethodExtractors.Select(e => e.GetImportedMethodInfos());
    }

    public static IEnumerable<ImportedMethodInfo> ToImportedMethodInfos(this IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> keyValuePairs)
    {
        return keyValuePairs.SelectMany(p => p.Value);
    }

    public static IEnumerable<string> ToDllLocations(this IEnumerable<ImportedMethodInfo> importedMethods)
    {
        return importedMethods.Select(m => m.DllLocation).Distinct();
    }

    public static IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> ToFileInfoImportedMethodInfosDictionary(this IEnumerable<IEnumerable<ImportedMethodInfo>> importedMethodInfos, IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Zip(importedMethodInfos, (fileInfo, importedMethods) => new
        {
            FileInfo = fileInfo,
            ImportedMethods = importedMethods
        })
        .ToDictionary(k => k.FileInfo, v => v.ImportedMethods);
    }
}