using Consyzer.Core.Models;

namespace Consyzer.Core.Checkers;

internal sealed class LibraryPresenceChecker(
    string analyzedDirectory
) : IFilePresenceChecker<LibraryPresence>
{
    private readonly Func<string, LibraryPresence?>[] _resolvers =
    [
        name => ResolveAnalyzedDirectory(analyzedDirectory, name),
        ResolveInEnvironmentPath,
        ResolveSystemDirectory,
        ResolveAbsolutePath,
        ResolveRelativePath
    ];

    public LibraryPresence Check(string file)
    {
        foreach (var resolver in _resolvers)
        {
            var presence = resolver(file);
            if (presence != null)
            {
                return presence;
            }
        }

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = null,
            LocationKind = LibraryLocationKind.Missing
        };
    }

    private static LibraryPresence? ResolveAnalyzedDirectory(string analyzedDirectory, string file)
    {
        var candidate = GetCandidatePath(analyzedDirectory, file);
        if (candidate == null || !IsPathInsideDirectory(analyzedDirectory, candidate)) return null;

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.InAnalyzedDirectory
        };
    }

    private static LibraryPresence? ResolveInEnvironmentPath(string file)
    {
        var candidate = GetCandidatePath(null, file);
        if (candidate == null) return null;

        var candidateDir = Path.GetFullPath(Path.GetDirectoryName(candidate) ?? string.Empty);
        var pathDirs = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(Path.GetFullPath);

        if (pathDirs.Any(dir => string.Equals(dir, candidateDir, StringComparison.OrdinalIgnoreCase)))
        {
            return new LibraryPresence
            {
                LibraryName = file,
                ResolvedPath = candidate,
                LocationKind = LibraryLocationKind.InEnvironmentPath
            };
        }
        return null;
    }

    private static LibraryPresence? ResolveSystemDirectory(string file)
    {
        var candidate = GetCandidatePath(Environment.SystemDirectory, file);
        if (candidate == null || !IsPathInsideDirectory(Environment.SystemDirectory, candidate)) return null;

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.InSystemDirectory
        };
    }

    private static LibraryPresence? ResolveAbsolutePath(string file)
    {
        var candidate = GetCandidatePath(null, file);
        if (candidate == null) return null;

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.OnAbsolutePath
        };
    }

    private static LibraryPresence? ResolveRelativePath(string file)
    {
        var candidate = GetCandidatePath(Directory.GetCurrentDirectory(), file);
        if (candidate == null) return null;

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.OnRelativePath
        };
    }

    private static string? GetCandidatePath(string? baseDir, string file)
    {
        string candidate = Path.IsPathRooted(file)
            ? file
            : baseDir is not null ? Path.Combine(baseDir, file) : file;
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