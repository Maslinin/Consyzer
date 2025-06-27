using Consyzer.Core.Models;

namespace Consyzer.Analyzers;

internal class ExitCodeAnalyzer : IAnalyzer<IEnumerable<LibraryPresence>, int>
{
    public int Analyze(IEnumerable<LibraryPresence> presences)
    {
        return presences.Max(p => (int)p.LocationKind);
    }
}