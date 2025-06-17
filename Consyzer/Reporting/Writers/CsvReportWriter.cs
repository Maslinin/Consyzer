using System.Text;
using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using static Consyzer.Constants;

namespace Consyzer.Reporting.Writers;

internal sealed class CsvReportWriter(
    IOptions<AppOptions> options
) : IReportWriter
{
    private AppOptions.ReportOptions.CsvOptions Options => options.Value.Report.Csv;

    public string Write(AnalysisOutcome outcome)
    {
        Directory.CreateDirectory(Report.Directory.FullPath);
        var fullPath = Path.Combine(Report.Directory.FullPath, Report.FileName.Csv);

        var encoding = GetEncoding(Options?.Encoding);
        var bom = encoding.GetPreamble();

        var csv = BuildCsv(outcome);

        File.WriteAllBytes(fullPath, [.. bom, .. encoding.GetBytes(csv)]);
        return fullPath;
    }

    private string BuildCsv(AnalysisOutcome outcome)
    {
        var sb = new StringBuilder();

        AppendAssemblyMetadataList(outcome, sb);
        AppendPInvokeGroups(outcome, sb);
        AppendLibraryPresences(outcome, sb);
        AppendSummary(outcome, sb);

        return sb.ToString();
    }

    private void AppendAssemblyMetadataList(AnalysisOutcome outcome, StringBuilder sb)
    {
        sb.AppendLine("[AssemblyMetadataList]");
        sb.AppendLine(GetDelimitedPropertyNames<AssemblyMetadata>());

        foreach (var m in outcome.AssemblyMetadataList)
        {
            sb.AppendLine(GetDelimitedPropertyValues(m));
        }

        sb.AppendLine();
    }

    private void AppendPInvokeGroups(AnalysisOutcome outcome, StringBuilder sb)
    {
        sb.AppendLine("[PInvokeGroups]");

        var sigProps = typeof(MethodSignature).GetProperties();
        var header = new List<string> { "File" };
        header.AddRange(sigProps.Select(p => $"Signature_{p.Name}"));
        header.AddRange(["ImportName", "ImportFlags"]);
        sb.AppendLine(string.Join(Options.Delimiter, header));

        foreach (var group in outcome.PInvokeGroups)
        {
            foreach (var method in group.Methods)
            {
                sb.AppendLine(string.Join(Options.Delimiter, GetPInvokeValues(group, method, sigProps)));
            }
        }

        sb.AppendLine();
    }

    private void AppendLibraryPresences(AnalysisOutcome outcome, StringBuilder sb)
    {
        sb.AppendLine("[LibraryPresences]");
        sb.AppendLine(GetDelimitedPropertyNames<LibraryPresence>());

        foreach (var l in outcome.LibraryPresences)
        {
            sb.AppendLine(GetDelimitedPropertyValues(l));
        }

        sb.AppendLine();
    }

    private void AppendSummary(AnalysisOutcome outcome, StringBuilder sb)
    {
        sb.AppendLine("[Summary]");
        sb.AppendLine(GetDelimitedPropertyNames<AnalysisSummary>());

        sb.AppendLine(GetDelimitedPropertyValues(outcome.Summary));

        sb.AppendLine();
    }

    private static IEnumerable<string> GetPInvokeValues(PInvokeMethodGroup group, PInvokeMethod method, PropertyInfo[] sigProps)
    {
        yield return Escape(group.File.FullName);

        var sig = method.Signature;
        foreach (var p in sigProps)
        {
            var val = p.GetValue(sig);
            if (val is IEnumerable<string> strList && val is not string)
                yield return Escape(string.Join(';', strList));
            else
                yield return Escape(val?.ToString() ?? string.Empty);
        }

        yield return Escape(method.ImportName);
        yield return Escape(method.ImportFlags.ToString());
    }

    private string GetDelimitedPropertyNames<T>()
    {
        return string.Join(Options.Delimiter, typeof(T).GetProperties().Select(p => p.Name));
    }

    private string GetDelimitedPropertyValues<T>(T obj)
    {
        var props = typeof(T).GetProperties();
        var values = props.Select(p =>
        {
            var val = p.GetValue(obj);
            if (val is IEnumerable<string> strList && val is not string)
            {
                return Escape(string.Join(Options.Delimiter, strList));
            }

            return Escape(val?.ToString() ?? string.Empty);
        });

        return string.Join(Options.Delimiter, values);
    }

    private static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "\"\"";
        }

        return $"\"{value.Replace("\"", "\"\"").Replace('\n', ' ').Replace('\r', ' ')}\"";
    }

    private static Encoding GetEncoding(string? name)
    {
        return string.IsNullOrEmpty(name) ? new UTF8Encoding(false) : Encoding.GetEncoding(name);
    }
}