using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Core.Checkers;

namespace Consyzer.Analyzers;

internal sealed class LibraryPresenceAnalyzer(
    IOptions<AnalysisOptions> analysisOptions
) : IAnalyzer<IEnumerable<string>, IEnumerable<LibraryPresence>>
{
    private readonly LibraryPresenceChecker _filePresenceChecker = new(analysisOptions.Value.AnalysisDirectory);

    public IEnumerable<LibraryPresence> Analyze(IEnumerable<string> fileNames)
    {
        foreach (var file in fileNames)
        {
            yield return this._filePresenceChecker.Check(file);
        }
    }

}