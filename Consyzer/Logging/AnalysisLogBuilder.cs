using Consyzer.Options;
using Consyzer.Core.Text;
using Consyzer.Core.Models;
using static Consyzer.Constants.OutputStructure;

namespace Consyzer.Logging;

internal sealed class AnalysisLogBuilder : IAnalysisLogBuilder
{
    public string BuildAnalysisOptionsLog(AnalysisOptions options) =>
        new IndentedTextBuilder()
            .Title(Section.AnalysisOptions)
            .PushIndent()
            .Line(Label.Options.AnalysisDirectory, options.AnalysisDirectory)
            .Line(Label.Options.SearchPattern, options.SearchPattern)
            .PopIndent()
            .Build();

    public string BuildFoundFilesLog(IEnumerable<FileInfo> files) =>
        new IndentedTextBuilder()
            .Title($"{Section.FilesFound} Count: {files.Count()}")
            .PushIndent()
            .IndexedItems(files, f => f.Name)
            .PopIndent()
            .Build();

    public string BuildFileClassificationLog(AnalysisFileClassification fileClassification) =>
        new IndentedTextBuilder()
            .Title(Section.FileClassification)
            .PushIndent()
            .Title($"{Section.NotEcma} Count: {fileClassification.NonEcmaModules.Count()}")
            .IndexedItems(fileClassification.NonEcmaModules, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"{Section.NotAssemblies} Count: {fileClassification.NonEcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.NonEcmaAssemblies, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"{Section.EcmaAssemblies} Count: {fileClassification.EcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.EcmaAssemblies, f => f.Name)
            .PopIndent()
            .Build();
}