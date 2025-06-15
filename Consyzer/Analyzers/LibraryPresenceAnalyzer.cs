using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Core.Checkers;

namespace Consyzer.Analyzers;

internal sealed class LibraryPresenceAnalyzer(
    IOptions<AnalysisOptions> analysisOptions
) : IAnalyzer<IEnumerable<PInvokeMethodGroup>, IEnumerable<LibraryPresence>>
{
    private readonly LibraryPresenceChecker _filePresenceChecker = new(analysisOptions.Value.AnalysisDirectory);

    public IEnumerable<LibraryPresence> Analyze(IEnumerable<PInvokeMethodGroup> methodGroups)
    {
        var distinctImportNames = methodGroups
            .SelectMany(g => g.Methods.Select(m => m.ImportName))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var import in distinctImportNames)
        {
            yield return this._filePresenceChecker.Check(import);
        }
    }
}