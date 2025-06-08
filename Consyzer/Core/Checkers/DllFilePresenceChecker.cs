using Consyzer.Core.Models;

namespace Consyzer.Core.Checkers;

internal sealed class DllFilePresenceChecker(
    string analyzedDirectory
) : IFilePresenceChecker<DllPresence>
{
    private readonly Func<string, DllPresence?>[] _resolvers =
    [
        name => ResolveAnalyzedDirectory(analyzedDirectory, name),
        ResolveInEnvironmentPath,
        ResolveSystemDirectory,
        ResolveAbsolutePath,
        ResolveRelativePath
    ];

    public DllPresence Check(string dllName)
    {
        foreach (var resolver in _resolvers)
        {
            var presence = resolver(dllName);
            if (presence != null)
            {
                return presence;
            }
        }

        return new DllPresence
        {
            DllName = dllName,
            ResolvedPath = null,
            LocationKind = DllLocationKind.Missing
        };
    }

    private static DllPresence? ResolveAnalyzedDirectory(string analyzedDirectory, string dllName)
    {
        var candidate = GetCandidatePath(analyzedDirectory, dllName);
        if (candidate == null || !IsPathInsideDirectory(analyzedDirectory, candidate)) return null;

        return new DllPresence
        {
            DllName = dllName,
            ResolvedPath = candidate,
            LocationKind = DllLocationKind.InAnalyzedDirectory
        };
    }

    private static DllPresence? ResolveInEnvironmentPath(string dllName)
    {
        var candidate = GetCandidatePath(null, dllName);
        if (candidate == null) return null;

        var candidateDir = Path.GetFullPath(Path.GetDirectoryName(candidate) ?? "");
        var pathDirs = (Environment.GetEnvironmentVariable("PATH") ?? "")
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(Path.GetFullPath);

        if (pathDirs.Any(dir => string.Equals(dir, candidateDir, StringComparison.OrdinalIgnoreCase)))
        {
            return new DllPresence
            {
                DllName = dllName,
                ResolvedPath = candidate,
                LocationKind = DllLocationKind.InEnvironmentPath
            };
        }
        return null;
    }

    private static DllPresence? ResolveSystemDirectory(string dllName)
    {
        var candidate = GetCandidatePath(Environment.SystemDirectory, dllName);
        if (candidate == null || !IsPathInsideDirectory(Environment.SystemDirectory, candidate)) return null;

        return new DllPresence
        {
            DllName = dllName,
            ResolvedPath = candidate,
            LocationKind = DllLocationKind.InSystemDirectory
        };
    }

    private static DllPresence? ResolveAbsolutePath(string dllName)
    {
        var candidate = GetCandidatePath(null, dllName);
        if (candidate == null) return null;

        return new DllPresence
        {
            DllName = dllName,
            ResolvedPath = candidate,
            LocationKind = DllLocationKind.OnAbsolutePath
        };
    }

    private static DllPresence? ResolveRelativePath(string dllName)
    {
        var candidate = GetCandidatePath(Directory.GetCurrentDirectory(), dllName);
        if (candidate == null) return null;

        return new DllPresence
        {
            DllName = dllName,
            ResolvedPath = candidate,
            LocationKind = DllLocationKind.OnRelativePath
        };
    }

    private static string? GetCandidatePath(string? baseDir, string dllName)
    {
        string candidate = Path.IsPathRooted(dllName)
            ? dllName
            : baseDir is not null ? Path.Combine(baseDir, dllName) : dllName;
        return File.Exists(candidate) ? candidate : null;
    }

    private static bool IsPathInsideDirectory(string baseDir, string path)
    {
        var baseFull = Path.GetFullPath(baseDir);
        var fileFull = Path.GetFullPath(path);
        var relative = Path.GetRelativePath(baseFull, fileFull);
        return !relative.StartsWith("..", StringComparison.OrdinalIgnoreCase);
    }
}