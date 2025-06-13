using Consyzer.Options;
using Consyzer.Core.Models;

namespace Consyzer.Logging;

internal sealed class AnalysisLogBuilder : IAnalysisLogBuilder
{
    public string BuildAnalysisOptionsLog(AnalysisOptions options) =>
        new LogBuilder()
            .Title("[Analysis Options]")
            .Line("Analysis Directory", options.AnalysisDirectory)
            .Line("Search Pattern", options.SearchPattern)
            .Build();

    public string BuildFoundFilesLog(IEnumerable<FileInfo> files) =>
        new LogBuilder()
            .Title($"[Files found] Count: {files.Count()}")
            .IndexedItems(files, f => f.Name)
            .Build();

    public string BuildFileClassificationLog(AnalysisFileClassification fileClassification) =>
        new LogBuilder()
            .Title("[File Classification]")
            .Title($"[Not ECMA] Count: {fileClassification.NonEcmaModules.Count()}")
            .IndexedItems(fileClassification.NonEcmaModules, f => f.Name)
            .Title($"[ECMA but not assemblies] Count: {fileClassification.NonEcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.NonEcmaAssemblies, f => f.Name)
            .Title($"[ECMA assemblies] Count: {fileClassification.EcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.EcmaAssemblies, f => f.Name)
            .Build();

    public string BuildEcmaAssemblyMetadataLog(IEnumerable<AssemblyMetadata> metadataList) =>
        new LogBuilder()
            .Title("[Assembly Metadata]")
            .IndexedSection(metadataList, (b, m) =>
            {
                b.InnerLine($"File: {m.File.Name}");
                b.InnerLine($"Version: {m.Version}");
                b.InnerLine($"CreationDate: {m.CreationDateUtc}");
                b.InnerLine($"SHA256 Hash: {m.Sha256}");
            })
            .Build();


    public string BuildPInvokeMethodGroupsLog(IEnumerable<PInvokeMethodGroup> groups) =>
        new LogBuilder()
            .Title("[P/Invoke Method Groups]")
            .Raw(BuildPInvokeMethodsGroupSections(groups))
            .Build();

    public string BuildDllPresenceLog(IEnumerable<DllPresence> presences) =>
        new LogBuilder()
            .Title("[DLL Presence Check]")
            .IndexedItems(presences, p =>
            {
                var status = p.LocationKind == DllLocationKind.Missing ? "MISSING" : $"FOUND [{p.LocationKind}]";
                return $"{p.DllName} — {status}";
            })
            .Build();

    public string BuildFinalSummaryLog(
        AnalysisFileClassification fileClassification,
        IEnumerable<DllPresence> dllPresenceList,
        IEnumerable<PInvokeMethodGroup> pinvokeGroups) =>
        new LogBuilder()
            .Title("[Final Summary]")
            .Line("Total Files", fileClassification.EcmaModules.Count() + fileClassification.NonEcmaModules.Count())
            .Line("ECMA Assemblies", fileClassification.EcmaAssemblies.Count())
            .Line("ECMA Assemblies with P/Invoke", pinvokeGroups.Count())
            .Line("Total P/Invoke Methods", pinvokeGroups.Sum(g => g.Methods.Count()))
            .Line("Missing DLLs", dllPresenceList.Count(p => p.LocationKind == DllLocationKind.Missing))
            .Build();

    private static string BuildPInvokeMethodsGroupSections(IEnumerable<PInvokeMethodGroup> groups) =>
        string.Join(Environment.NewLine, groups.Select((group, index) =>
            new LogBuilder()
                .Title($"[{index}] File: {group.File.Name} — Found: {group.Methods.Count()}")
                .IndexedSection(group.Methods, (b, m) =>
                {
                    b.InnerLine($"Method Signature: '{m.Signature}'");
                    b.InnerLine($"Import Name: '{m.ImportName}'");
                    b.InnerLine($"Import Flags: '{m.ImportFlags}'");
                })
                .Build()));
}