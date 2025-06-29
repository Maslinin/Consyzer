using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Output.Formatters;
using static Consyzer.Constants.Output.Structure;

namespace Consyzer.Output.Logging;

internal sealed class AnalysisLogBuilder : IAnalysisLogBuilder
{
    public string BuildAnalysisOptionsLog(AnalysisOptions options) =>
        new IndentedTextBuilder()
            .Title(Section.Bracketed.AnalysisOptions)
            .PushIndent()
            .Line(Label.Options.AnalysisDirectory, options.AnalysisDirectory)
            .Line(Label.Options.SearchPattern, options.SearchPattern)
            .PopIndent()
            .Build();

    public string BuildFoundFilesLog(IEnumerable<FileInfo> files) =>
        new IndentedTextBuilder()
            .Title($"{Section.Bracketed.FilesFound} Count: {files.Count()}")
            .PushIndent()
            .IndexedItems(files, f => f.Name)
            .PopIndent()
            .Build();

    public string BuildFileClassificationLog(AnalysisFileClassification fileClassification) =>
        new IndentedTextBuilder()
            .Title(Section.Bracketed.FileClassification)
            .PushIndent()
            .Title($"{Section.Bracketed.NotEcma} Count: {fileClassification.NonEcmaModules.Count()}")
            .IndexedItems(fileClassification.NonEcmaModules, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"{Section.Bracketed.NotAssemblies} Count: {fileClassification.NonEcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.NonEcmaAssemblies, f => f.Name)
            .PopIndent()
            .PushIndent()
            .Title($"{Section.Bracketed.EcmaAssemblies} Count: {fileClassification.EcmaAssemblies.Count()}")
            .IndexedItems(fileClassification.EcmaAssemblies, f => f.Name)
            .PopIndent()
            .Build();
}