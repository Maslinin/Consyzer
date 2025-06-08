using Consyzer.Core.Models;
using Consyzer.Core.Extractors;

namespace Consyzer.Analyzers;

internal sealed class PInvokeMethodAnalyzer(
    IExtractor<FileInfo, IEnumerable<PInvokeMethodsGroup>> pInvokeMethodExtractor
) : IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodsGroup>>
{
    public IEnumerable<PInvokeMethodsGroup> Analyze(IEnumerable<FileInfo> files)
    {
        foreach (var file in files)
        {
            foreach (var methodsGroup in pInvokeMethodExtractor.Extract(file))
            {
                yield return methodsGroup;
            }
        }
    }
}