using Consyzer.Core.Text;
using Consyzer.Core.Models;
using static Consyzer.Reporting.Writers.Sections.ReportSections;

namespace Consyzer.Reporting.Writers;

internal sealed class ConsoleReportWriter : IReportWriter
{
    private const string Destination = "Console";

    public string Write(AnalysisOutcome outcome)
    {
        WriteAssemblyMetadata(outcome.AssemblyMetadataList);
        WritePInvokeGroups(outcome.PInvokeGroups);
        WriteLibraryPresence(outcome.LibraryPresences);
        WriteSummary(outcome.Summary);

        return Destination;
    }

    private static void WriteAssemblyMetadata(IEnumerable<AssemblyMetadata> metadataList)
    {
        var builder = new IndentedTextBuilder()
            .Title(AssemblyMetadataList)
            .PushIndent()
            .IndexedSection(metadataList, (b, m) =>
            {
                b.Line($"File: {m.File.Name}");
                b.Line($"Version: {m.Version}");
                b.Line($"CreationDate: {m.CreationDateUtc}");
                b.Line($"SHA256 Hash: {m.Sha256}");
            })
            .PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WritePInvokeGroups(IEnumerable<PInvokeMethodGroup> groups)
    {
        var builder = new IndentedTextBuilder()
            .Title(PInvokeGroups)
            .PushIndent();

        builder.IndexedSection(groups, (b, g) =>
        {
            var methods = g.Methods;

            if (!methods.Any())
            {
                b.Line($"File: {g.File.Name} — No P/Invoke methods");
                return;
            }

            b.Line($"File: {g.File.Name} — Found: {methods.Count()}");

            b.IndexedSection(methods, (bb, m) =>
            {
                bb.Line($"Method Signature: '{m.Signature}'");
                bb.Line($"Import Name: '{m.ImportName}'");
                bb.Line($"Import Flags: '{m.ImportFlags}'");
            });
        });

        builder.PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WriteLibraryPresence(IEnumerable<LibraryPresence> presences)
    {
        var builder = new IndentedTextBuilder()
            .Title(LibraryPresences)
            .PushIndent()
            .IndexedItems(presences, p =>
            {
                var status = p.LocationKind == LibraryLocationKind.Missing ? "MISSING" : "FOUND";
                return $"{p.LibraryName} — {status} [{p.LocationKind}]";
            })
            .PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WriteSummary(AnalysisSummary summary)
    {
        var builder = new IndentedTextBuilder()
            .Title(Summary)
            .PushIndent()
            .Line("Total Files", summary.TotalFiles)
            .Line("ECMA Assemblies", summary.EcmaAssemblies)
            .Line("Assemblies With P/Invoke", summary.AssembliesWithPInvoke)
            .Line("Total P/Invoke Methods", summary.TotalPInvokeMethods)
            .Line("Missing Libraries", summary.MissingLibraries)
            .PopIndent();

        Console.Out.Write(builder.Build());
    }
}