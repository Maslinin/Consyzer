using Consyzer.Options;
using Consyzer.Core.Text;
using Consyzer.Core.Models;

namespace Consyzer.Logging;

internal sealed class AnalysisLogBuilder : IAnalysisLogBuilder
{
    public string BuildAnalysisOptionsLog(AnalysisOptions options) =>
        new IndentedTextBuilder()
            .Title("[Analysis Options]")
            .PushIndent()
            .Line("Analysis Directory", options.AnalysisDirectory)
            .Line("Search Pattern", options.SearchPattern)
            .PopIndent()
            .Build();

    public string BuildFoundFilesLog(IEnumerable<FileInfo> files) =>
        new IndentedTextBuilder()
            .Title($"[Files found] Count: {files.Count()}")
            .PushIndent()
            .IndexedItems(files, f => f.Name)
            .PopIndent()
            .Build();

    public string BuildFileClassificationLog(AnalysisFileClassification fileClassification) =>
        new IndentedTextBuilder()
            .Title("[File Classification]")
            .PushIndent()
            .Title($"[Not ECMA] Count: {fileClassification.NonEcmaModules.Count()}")
            .IndexedItems(fileClassification.NonEcmaModules, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"[ECMA but not assemblies] Count: {fileClassification.NonEcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.NonEcmaAssemblies, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"[ECMA assemblies] Count: {fileClassification.EcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.EcmaAssemblies, f => f.Name)
            .PopIndent()
            .Build();
}