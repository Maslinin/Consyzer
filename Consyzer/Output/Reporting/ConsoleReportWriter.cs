using Consyzer.Core.Models;
using Consyzer.Output.Formatters;
using static Consyzer.Constants.Output;
using static Consyzer.Constants.Output.Structure;

namespace Consyzer.Output.Reporting;

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
            .Title(Section.Bracketed.AssemblyMetadataList)
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
            .Title(Section.Bracketed.PInvokeMethodGroups)
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
            .Title(Section.Bracketed.LibraryPresences)
            .PushIndent()
            .IndexedSection(presences, (b, p) =>
            {
                b.Line(Label.Library.Name, p.LibraryName);
                b.Line(Label.Library.ResolvedPath, p.ResolvedPath ?? "null");
                b.Line(Label.Library.LocationKind, p.LocationKind);
            })
            .PopIndent();

        Console.Out.Write(builder.Build());
    }

    private static void WriteSummary(AnalysisSummary summary)
    {
        var builder = new IndentedTextBuilder()
            .Title(Section.Bracketed.Summary)
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