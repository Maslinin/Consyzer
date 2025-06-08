using Consyzer.Core.Models;

namespace Consyzer.Analyzers;

internal class ExitCodeAnalyzer : IAnalyzer<IEnumerable<DllPresence>, int>
{
    public int Analyze(IEnumerable<DllPresence> presences)
    {
        return presences.Max(p => (int)p.LocationKind);
    }
}
