using Consyzer.Core.Models;
using static Consyzer.Constants.LibraryPresence;

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

    public LibraryPresence Check(string importName)
    {
        foreach (var candidateName in ResolveLibraryName(importName))
        {
            foreach (var resolver in this._resolvers)
            {
                var presence = resolver(candidateName);
                if (presence is not null)
                {
                    return presence with { LibraryName = importName };
                }
            }
        }

        return new LibraryPresence
        {
            LibraryName = importName,
            ResolvedPath = null,
            LocationKind = LibraryLocationKind.Missing
        };
    }

    private static IEnumerable<string> ResolveLibraryName(string importName)
    {
        if (Path.HasExtension(importName))
        {
            yield return importName;
            yield break;
        }

        if (OperatingSystem.IsWindows())
            yield return importName + Extension.WindowsExtension;
        else if (OperatingSystem.IsLinux())
            yield return importName + Extension.LinuxExtension;
        else if (OperatingSystem.IsMacOS())
            yield return importName + Extension.MacExtension;
        else yield return importName;
    }

    private static LibraryPresence? ResolveAnalyzedDirectory(string analyzedDirectory, string importName)
    {
        var candidate = GetCandidatePath(analyzedDirectory, importName);
        if (candidate == null || !IsPathInsideDirectory(analyzedDirectory, candidate)) return null;

        return new LibraryPresence
        {
            LibraryName = importName,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.InAnalyzedDirectory
        };
    }

    private static LibraryPresence? ResolveInEnvironmentPath(string importName)
    {
        var candidate = GetCandidatePath(null, importName);
        if (candidate == null) return null;

        var candidateDir = Path.GetFullPath(Path.GetDirectoryName(candidate) ?? string.Empty);
        var pathDirs = (Environment.GetEnvironmentVariable(Variable.Path) ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(Path.GetFullPath);

        if (pathDirs.Any(dir => string.Equals(dir, candidateDir, StringComparison.OrdinalIgnoreCase)))
        {
            return new LibraryPresence
            {
                LibraryName = importName,
                ResolvedPath = candidate,
                LocationKind = LibraryLocationKind.InEnvironmentPath
            };
        }

        return null;
    }

    private static LibraryPresence? ResolveSystemDirectory(string importName)
    {
        var candidate = GetCandidatePath(Environment.SystemDirectory, importName);
        if (candidate == null || !IsPathInsideDirectory(Environment.SystemDirectory, candidate)) return null;

        return new LibraryPresence
        {
            LibraryName = importName,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.InSystemDirectory
        };
    }

    private static LibraryPresence? ResolveAbsolutePath(string importName)
    {
        var candidate = GetCandidatePath(null, importName);
        if (candidate == null) return null;

        return new LibraryPresence
        {
            LibraryName = importName,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.OnAbsolutePath
        };
    }

    private static LibraryPresence? ResolveRelativePath(string importName)
    {
        var candidate = GetCandidatePath(Directory.GetCurrentDirectory(), importName);
        if (candidate == null) return null;

        return new LibraryPresence
        {
            LibraryName = importName,
            ResolvedPath = candidate,
            LocationKind = LibraryLocationKind.OnRelativePath
        };
    }

    private static string? GetCandidatePath(string? baseDir, string importName)
    {
        string candidate = Path.IsPathRooted(importName)
            ? importName
            : baseDir is not null ? Path.Combine(baseDir, importName) : importName;

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