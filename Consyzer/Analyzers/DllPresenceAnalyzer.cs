using Microsoft.Extensions.Options;
using Consyzer.Options;
using Consyzer.Core.Models;
using Consyzer.Core.Checkers;

namespace Consyzer.Analyzers;

internal sealed class DllPresenceAnalyzer(
    IOptions<AnalysisOptions> analysisOptions
) : IAnalyzer<IEnumerable<string>, IEnumerable<DllPresence>>
{
    private readonly DllFilePresenceChecker _filePresenceChecker = new(analysisOptions.Value.AnalysisDirectory);

    public IEnumerable<DllPresence> Analyze(IEnumerable<string> dllNames)
    {
        foreach (var dllName in dllNames)
        {
            yield return this._filePresenceChecker.Check(dllName);
        }
    }

}