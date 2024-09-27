using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal interface IEcmaImportedMethodExtractor
{
    FileInfo MetadataAssembly { get; }
    IEnumerable<ImportedMethodInfo> GetImportedMethodInfos();
}