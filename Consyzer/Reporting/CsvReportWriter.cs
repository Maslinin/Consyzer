using System.Text;
using Consyzer.Options;
using Consyzer.Core.Text;
using Consyzer.Core.Models;
using Microsoft.Extensions.Options;
using static Consyzer.Constants;
using static Consyzer.Constants.OutputStructure;

namespace Consyzer.Reporting;

internal sealed class CsvReportWriter(
    IOptions<AppOptions> options
) : IReportWriter
{
    private AppOptions.ReportOptions.CsvOptions Options => options.Value.Report.Csv;

    public string Write(AnalysisOutcome outcome)
    {
        Directory.CreateDirectory(Report.Directory.FullPath);
        var fullPath = Path.Combine(Report.Directory.FullPath, Report.Name.Csv);

        var encoding = GetEncoding(Options?.Encoding);
        var bom = encoding.GetPreamble();

        var csv = BuildCsv(outcome);

        File.WriteAllBytes(fullPath, [.. bom, .. encoding.GetBytes(csv)]);
        return fullPath;
    }

    private string BuildCsv(AnalysisOutcome outcome)
    {
        var sb = new StringBuilder();

        sb.AppendLine(Section.AssemblyMetadataList);
        sb.AppendLine(CsvTable(outcome.AssemblyMetadataList));

        sb.AppendLine(Section.PInvokeMethodGroups);
        sb.AppendLine(CsvPInvoke(outcome.PInvokeMethodGroups));

        sb.AppendLine(Section.LibraryPresences);
        sb.AppendLine(CsvTable(outcome.LibraryPresences));

        sb.AppendLine(Section.Summary);
        sb.AppendLine(CsvTable([outcome.Summary]));

        return sb.ToString();
    }

    private string CsvTable<T>(IEnumerable<T> items)
    {
        return new CsvTableBuilder(Options.Delimiter)
            .Records(items, SerializeValue)
            .ToString();
    }

    private string CsvPInvoke(IEnumerable<PInvokeMethodGroup> methodGroups)
    {
        var signatureProperties = typeof(MethodSignature).GetProperties();
        var signatureFieldName = nameof(PInvokeMethod.Signature);

        var header = new List<string>
        {
            Label.PInvoke.File
        };

        header.AddRange(signatureProperties.Select(p => $"{signatureFieldName}_{p.Name}"));
        header.Add(Label.PInvoke.ImportName);
        header.Add(Label.PInvoke.ImportFlags);

        var table = new CsvTableBuilder(Options.Delimiter)
            .Header(header);

        foreach (var group in methodGroups)
        {
            foreach (var method in group.Methods)
            {
                var record = new List<string>
                {
                    SerializeValue(group.File.FullName)
                };

                record.AddRange(signatureProperties.Select(p => SerializeValue(p.GetValue(method.Signature))));
                record.Add(SerializeValue(method.ImportName));
                record.Add(SerializeValue(method.ImportFlags.ToString()));

                table.Record(record);
            }
        }

        return table.ToString();
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
        var delimiter = Options.Delimiter;
        var innerDelimiter = GetSafeInnerDelimiter(delimiter);
        var safeItems = items.Select(s => (s ?? string.Empty).Replace(delimiter, ' '));
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

    private static Encoding GetEncoding(string? encoding)
    {
        return string.IsNullOrEmpty(encoding) ? new UTF8Encoding(false) : Encoding.GetEncoding(encoding);
    }
}