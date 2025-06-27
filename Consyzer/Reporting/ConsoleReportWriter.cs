using Consyzer.Core.Text;
using Consyzer.Core.Models;
using static Consyzer.Constants.OutputStructure;

namespace Consyzer.Reporting;

internal sealed class ConsoleReportWriter : IReportWriter
{
    private const string Destination = "Console";

    public string Write(AnalysisOutcome outcome)
    {
        WriteAssemblyMetadata(outcome.AssemblyMetadataList);
        WritePInvokeGroups(outcome.PInvokeMethodGroups);
        WriteLibraryPresence(outcome.LibraryPresences);
        WriteSummary(outcome.Summary);

        return Destination;
    }

    private static void WriteAssemblyMetadata(IEnumerable<AssemblyMetadata> assemblyInfos)
    {
        var builder = new IndentedTextBuilder()
            .Title(Section.AssemblyMetadataList)
            .PushIndent()
            .IndexedSection(assemblyInfos, (b, m) =>
            {
                b.Line($"{Label.Assembly.File}: {m.File.Name}");
                b.Line($"{Label.Assembly.Version}: {m.Version}");
                b.Line($"{Label.Assembly.CreationDateUtc}: {m.CreationDateUtc}");
                b.Line($"{Label.Assembly.Sha256}: {m.Sha256}");
            })
            .PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WritePInvokeGroups(IEnumerable<PInvokeMethodGroup> groups)
    {
        var builder = new IndentedTextBuilder()
            .Title(Section.PInvokeMethodGroups)
            .PushIndent();

        builder.IndexedSection(groups, (b, g) =>
        {
            b.Line($"{Label.PInvoke.File}: {g.File.Name} — Found: {g.Methods.Count()}");

            b.IndexedSection(g.Methods, (bb, m) =>
            {
                bb.Line($"{Label.PInvoke.Signature}: '{m.Signature}'");
                bb.Line($"{Label.PInvoke.ImportName}: '{m.ImportName}'");
                bb.Line($"{Label.PInvoke.ImportFlags}: '{m.ImportFlags}'");
            });
        });

        builder.PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WriteLibraryPresence(IEnumerable<LibraryPresence> presences)
    {
        var builder = new IndentedTextBuilder()
            .Title(Section.LibraryPresences)
            .PushIndent()
            .IndexedItems(presences, p =>
            {
                var presence = p.LocationKind == LibraryLocationKind.Missing ? "MISSING" : "FOUND";
                return $"{p.LibraryName} — {presence} [{p.LocationKind}]";
            })
            .PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WriteSummary(AnalysisSummary summary)
    {
        var builder = new IndentedTextBuilder()
            .Title(Section.Summary)
            .PushIndent()
            .Line(Label.Summary.TotalFiles, summary.TotalFiles)
            .Line(Label.Summary.EcmaAssemblies, summary.EcmaAssemblies)
            .Line(Label.Summary.AssembliesWithPInvoke, summary.AssembliesWithPInvoke)
            .Line(Label.Summary.TotalPInvokeMethods, summary.TotalPInvokeMethods)
            .Line(Label.Summary.MissingLibraries, summary.MissingLibraries)
            .PopIndent();

        Console.Out.Write(builder.Build());
    }
}