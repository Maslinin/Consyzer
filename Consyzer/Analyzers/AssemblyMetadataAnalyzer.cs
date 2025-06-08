using Consyzer.Core.Models;
using Consyzer.Core.Extractors;

namespace Consyzer.Analyzers;

internal sealed class AssemblyMetadataAnalyzer(
    IExtractor<FileInfo, AssemblyMetadata> extractor
) : IAnalyzer<IEnumerable<FileInfo>, IEnumerable<AssemblyMetadata>>
{
    public IEnumerable<AssemblyMetadata> Analyze(IEnumerable<FileInfo> files)
    {
        foreach (var file in files)
        {
            yield return extractor.Extract(file);
        }
    }
}
