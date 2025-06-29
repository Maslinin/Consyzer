using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Reflection;
using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Output.Reporting.Converters;
using static Consyzer.Constants.Output;

namespace Consyzer.Output.Reporting;

internal sealed class JsonReportWriter(
    IOptions<AppOptions> options
) : IReportWriter
{
    private readonly AppOptions.OutputOptions.JsonOptions _options = options.Value.Output.Json;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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

        var encoding = Encoding.GetEncoding(_options.Encoding);

        var json = JsonSerializer.Serialize(outcome, JsonOptions);
        File.WriteAllText(fullPath, json, encoding);

        return fullPath;
    }
}