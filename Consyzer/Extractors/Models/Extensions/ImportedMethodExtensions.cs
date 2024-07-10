namespace Consyzer.Extractors.Models.Extensions;

internal static class ImportedMethodExtensions
{
    public static IDictionary<FileInfo, IEcmaImportedMethodExtractor> ToImportedMethodExtractors(this IEnumerable<FileInfo> metadataAssemblyFiles)
    {
        return metadataAssemblyFiles.ToDictionary(k => k, v => (IEcmaImportedMethodExtractor)new MetadataImportedMethodExtractor(v));
    }

    public static IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> ToImportedMethodInfos(this IDictionary<FileInfo, IEcmaImportedMethodExtractor> importedMethodExtractors)
    {
        return importedMethodExtractors.ToDictionary(k => k.Key, v => v.Value.GetImportedMethodInfos());
    }

    public static IEnumerable<string> ToDllLocations(this IDictionary<FileInfo, IEnumerable<ImportedMethodInfo>> importedMethodInfos)
    {
        return importedMethodInfos.SelectMany(m => m.Value.Select(i => i.DllLocation)).Distinct();
    }
}