using Consyzer.Core.Models;

namespace Consyzer.Reporting;

internal interface IReportWriter
{
    string WriteReport(AnalysisOutcome outcome);
}
