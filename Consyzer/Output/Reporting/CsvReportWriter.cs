using System.Text;
using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Output.Builders;
using static Consyzer.Constants.Output;

namespace Consyzer.Output.Reporting;

internal sealed class CsvReportWriter(
    IOptions<AppOptions> options
) : IReportWriter
{
    private readonly AppOptions.OutputOptions.CsvOptions _options = options.Value.Output.Csv;

    public string Write(AnalysisOutcome outcome)
    {
        Directory.CreateDirectory(Destination.TargetDirectory);
        var fullPath = Path.Combine(Destination.TargetDirectory, Destination.Csv);

        var encoding = Encoding.GetEncoding(_options.Encoding);
        var builder = new CsvTableBuilder(_options.Delimiter);

        WriteAssemblyMetadata(builder, outcome.AssemblyMetadataList);
        WritePInvokeGroups(builder, outcome.PInvokeMethodGroups);
        WriteLibraryPresences(builder, outcome.LibraryPresences);
        WriteSummary(builder, outcome.Summary);

        File.WriteAllText(fullPath, builder.Build(), encoding);

        return fullPath;
    }

    private void WriteAssemblyMetadata(CsvTableBuilder builder, IEnumerable<AssemblyMetadata> metadataList)
    {
        builder.Record([Structure.Section.Bracketed.AssemblyMetadataList]);
        builder.Records(metadataList, SerializeValue);
        builder.Record([]);
    }

    private void WritePInvokeGroups(CsvTableBuilder builder, IEnumerable<PInvokeMethodGroup> groups)
    {
        builder.Record([Structure.Section.Bracketed.PInvokeMethodGroups]);

        var signatureProperties = typeof(MethodSignature).GetProperties();
        var signaturePrefix = nameof(PInvokeMethod.Signature);

        var header = new List<string> { Structure.Label.PInvoke.File };
        header.AddRange(signatureProperties.Select(p => $"{signaturePrefix}_{p.Name}"));
        header.Add(Structure.Label.PInvoke.ImportName);
        header.Add(Structure.Label.PInvoke.ImportFlags);

        builder.Header(header);

        foreach (var group in groups)
        {
            foreach (var method in group.Methods)
            {
                var record = new List<string> { SerializeValue(group.File.FullName) };
                record.AddRange(signatureProperties.Select(p => SerializeValue(p.GetValue(method.Signature))));
                record.Add(SerializeValue(method.ImportName));
                record.Add(SerializeValue(method.ImportFlags.ToString()));

                builder.Record(record);
            }
        }

        builder.Record([]);
    }

    private void WriteLibraryPresences(CsvTableBuilder builder, IEnumerable<LibraryPresence> presences)
    {
        builder.Record([Structure.Section.Bracketed.LibraryPresences]);
        builder.Records(presences, SerializeValue);
        builder.Record([]);
    }

    private void WriteSummary(CsvTableBuilder builder, AnalysisSummary summary)
    {
        builder.Record([Structure.Section.Bracketed.Summary]);
        builder.Records([summary], SerializeValue);
        builder.Record([]);
    }

    private string SerializeValue(object? value)
    {
        if (value is IEnumerable<string> strList && value is not string)
        {
            return EscapeList(strList);
        }

        return EscapeValue(value?.ToString());
    }

    private string EscapeList(IEnumerable<string> items)
    {
        var delimiter = _options.Delimiter;
        var innerDelimiter = GetSafeInnerDelimiter(delimiter);
        var safeItems = items.Select(i => (i ?? string.Empty).Replace(delimiter, ' '));
        var joined = string.Join(innerDelimiter, safeItems);
        return EscapeValue(joined);
    }

    private static char GetSafeInnerDelimiter(char delimiter)
        => delimiter switch
        {
            ';' => '|',
            '|' => '/',
            ',' => ';',
            _ => ' '
        };

    private static string EscapeValue(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "\"\"";
        return $"\"{value.Replace("\"", "\"\"").Replace('\n', ' ').Replace('\r', ' ')}\"";
    }
}