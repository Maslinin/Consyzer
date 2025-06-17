using System.Text.Json;
using System.Reflection;
using Consyzer.Core.Models;
using static Consyzer.Constants;
using Consyzer.Reporting.Writers.Converters;

namespace Consyzer.Reporting.Writers;

internal sealed class JsonReportWriter : IReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = 
        { 
            new JsonFileInfoConverter(), 
            new JsonEnumConverter<LibraryLocationKind>(),
            new JsonEnumConverter<MethodImportAttributes>()
        }
    };

    public string Write(AnalysisOutcome outcome)
    {
        Directory.CreateDirectory(Report.Directory.FullPath);

        var fullPath = Path.Combine(Report.Directory.FullPath, Report.FileName.Json);

        var json = JsonSerializer.Serialize(outcome, JsonOptions);
        File.WriteAllText(fullPath, json);

        return fullPath;
    }
}