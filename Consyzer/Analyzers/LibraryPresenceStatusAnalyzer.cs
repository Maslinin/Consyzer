using Consyzer.Core.Models;

namespace Consyzer.Analyzers;

internal sealed class LibraryPresenceStatusAnalyzer : IAnalyzer<IEnumerable<LibraryPresence>, LibraryLocationKind>
{
    public LibraryLocationKind Analyze(IEnumerable<LibraryPresence> presences)
    {
        return presences.Max(p => p.LocationKind);
    }
}