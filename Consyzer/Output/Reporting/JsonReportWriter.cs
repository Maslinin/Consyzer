using System.Text.Json;
using System.Reflection;
using Consyzer.Core.Models;
using static Consyzer.Constants.Output;
using Consyzer.Output.Reporting.Converters;

namespace Consyzer.Output.Reporting;

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
        Directory.CreateDirectory(Destination.TargetDirectory);
        var fullPath = Path.Combine(Destination.TargetDirectory, Destination.Json);

        var json = JsonSerializer.Serialize(outcome, JsonOptions);
        File.WriteAllText(fullPath, json);

        return fullPath;
    }
}