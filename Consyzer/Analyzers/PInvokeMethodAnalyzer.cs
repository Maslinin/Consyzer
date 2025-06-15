using Consyzer.Core.Models;
using Consyzer.Core.Extractors;

namespace Consyzer.Analyzers;

internal sealed class PInvokeMethodAnalyzer(
    IExtractor<FileInfo, IEnumerable<PInvokeMethod>> pInvokeMethodExtractor
) : IAnalyzer<IEnumerable<FileInfo>, IEnumerable<PInvokeMethodGroup>>
{
    public IEnumerable<PInvokeMethodGroup> Analyze(IEnumerable<FileInfo> files)
    {
        foreach (var file in files)
        {
            var methods = pInvokeMethodExtractor.Extract(file);
            if (!methods.Any())
            {
                continue;
            }

            yield return new PInvokeMethodGroup
            {
                File = file,
                Methods = methods
            };
        }
    }
}