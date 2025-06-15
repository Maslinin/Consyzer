using Consyzer.Core.Text;
using Consyzer.Core.Models;

namespace Consyzer.Reporting.Writers;

internal sealed class ConsoleReportWriter : IReportWriter
{
    private const string Destination = "Console";

    public string WriteReport(AnalysisOutcome outcome)
    {
        WriteAssemblyMetadata(outcome.AssemblyMetadataList);
        WritePInvokeGroups(outcome.PInvokeGroups, outcome.AssemblyMetadataList);
        WriteLibraryPresence(outcome.LibraryPresences);
        WriteSummary(outcome.Summary);

        return Destination;
    }

    private static void WriteAssemblyMetadata(IEnumerable<AssemblyMetadata> metadataList)
    {
        var builder = new IndentedTextBuilder()
            .Title("[Assembly Metadata]")
            .IndexedSection(metadataList, (b, m) =>
            {
                b.InnerLine($"File: {m.File.Name}");
                b.InnerLine($"Version: {m.Version}");
                b.InnerLine($"CreationDate: {m.CreationDateUtc}");
                b.InnerLine($"SHA256 Hash: {m.Sha256}");
            });

        Console.Out.Write(builder.Build());
    }

    private static void WritePInvokeGroups(IEnumerable<PInvokeMethodGroup> groups, IEnumerable<AssemblyMetadata> metadata)
    {
        var builder = new IndentedTextBuilder()
            .Title("[P/Invoke Method Groups]");

        int index = 0;

        var allFiles = metadata.Select(m => m.File);

        foreach (var file in allFiles)
        {
            var group = groups.FirstOrDefault(g => g.File.Name.Equals(file.Name, StringComparison.OrdinalIgnoreCase));

            if (group is not null && group.Methods.Any())
            {
                builder.Title($"[{index++}] File: {file.Name} — Found: {group.Methods.Count()}");
                builder.IndexedSection(group.Methods, (b, m) =>
                {
                    b.InnerLine($"Method Signature: '{m.Signature}'");
                    b.InnerLine($"Import Name: '{m.ImportName}'");
                    b.InnerLine($"Import Flags: '{m.ImportFlags}'");
                });
            }
            else
            {
                builder.Title($"[{index++}] {file.Name} - No P/Invoke methods");
            }
        }

        Console.Out.Write(builder.Build());
    }

    private static void WriteLibraryPresence(IEnumerable<LibraryPresence> presences)
    {
        var builder = new IndentedTextBuilder()
            .Title("[Library Presence Check]")
            .IndexedItems(presences, p =>
            {
                var status = p.LocationKind == LibraryLocationKind.Missing ? "MISSING" : $"FOUND [{p.LocationKind}]";
                return $"{p.LibraryName} — {status}";
            });

        Console.Out.Write(builder.Build());
    }

    private static void WriteSummary(AnalysisSummary summary)
    {
        var builder = new IndentedTextBuilder()
            .Title("[Analysis Summary]")
            .Line("Total Files", summary.TotalFiles)
            .Line("ECMA Assemblies", summary.EcmaAssemblies)
            .Line("Assemblies With P/Invoke", summary.AssembliesWithPInvoke)
            .Line("Total P/Invoke Methods", summary.TotalPInvokeMethods)
            .Line("Missing Libraries", summary.MissingLibraries);

        Console.Out.Write(builder.Build());
    }
}