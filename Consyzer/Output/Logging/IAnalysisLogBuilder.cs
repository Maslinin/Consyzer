using Consyzer.Options;
using Consyzer.Core.Models;

namespace Consyzer.Output.Logging;

internal interface IAnalysisLogBuilder
{
    string BuildAnalysisOptionsLog(AnalysisOptions options);
    string BuildFoundFilesLog(IEnumerable<FileInfo> files);
    string BuildFileClassificationLog(AnalysisFileClassification fileClassification);

}