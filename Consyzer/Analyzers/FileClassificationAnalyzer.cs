using Consyzer.Core.Models;
using Consyzer.Core.Checkers;

namespace Consyzer.Analyzers;

internal sealed class FileClassificationAnalyzer(
    IFileClassificationChecker<AnalysisFileClassification> fileClassificationChecker
) : IAnalyzer<IEnumerable<FileInfo>, AnalysisFileClassification>
{
    public AnalysisFileClassification Analyze(IEnumerable<FileInfo> files)
    {
        return fileClassificationChecker.Check(files);
    }
}