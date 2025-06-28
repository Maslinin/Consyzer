using Consyzer.Core.Models;

namespace Consyzer.Output.Reporting;

internal interface IReportWriter
{
    string Write(AnalysisOutcome outcome);
}