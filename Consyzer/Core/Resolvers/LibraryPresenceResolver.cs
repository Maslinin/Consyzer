using Consyzer.Core.Models;
using static Consyzer.Constants.LibraryPresence;

namespace Consyzer.Core.Resolvers;

internal sealed class LibraryPresenceResolver(
    string analyzedDirectory
) : IFilePresenceResolver<LibraryPresence>
{
    private readonly Func<string, LibraryPresence?>[] _resolvers =
    [
        name => ResolveAnalyzedDirectory(analyzedDirectory, name),
        ResolveSystemDirectory,
        ResolveInEnvironmentPath,
        ResolveAbsolutePath,
        ResolveRelativePath
    ];

    public LibraryPresence Check(string file)
    {
        foreach (var candidateName in ResolveLibraryName(file))
        {
            foreach (var resolver in _resolvers)
            {
                var presence = resolver(candidateName);
                if (presence is not null)
                {
                    return presence;
                }
            }
        }

        return new LibraryPresence
        {
            LibraryName = file,
            ResolvedPath = null,
            LocationKind = LibraryLocationKind.Missing
        };
    }

    private static IEnumerable<string> ResolveLibraryName(string file)
    {
        if (Path.HasExtension(file))
        {
            yield return file;
            yield break;
        }

        if (OperatingSystem.IsWindows())
            yield return file + Extension.WindowsExtension;
        else if (OperatingSystem.IsLinux())
            yield return file + Extension.LinuxExtension;
        else if (OperatingSystem.IsMacOS())
            yield return file + Extension.MacExtension;
        else yield return file;
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
        if (Path.IsPathRooted(file) || Path.GetFileName(file) != file)
            return null;

        var pathDirs = (Environment.GetEnvironmentVariable(Variable.Path) ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

        foreach (var dir in pathDirs)
        {
            var candidate = Path.Combine(dir, file);
            if (File.Exists(candidate))
            {
                return new LibraryPresence
                {
                    LibraryName = file,
                    ResolvedPath = Path.GetFullPath(candidate),
                    LocationKind = LibraryLocationKind.InEnvironmentPath
                };
            }
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
        if (!Path.IsPathRooted(file)) return null;

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
        if (Path.IsPathRooted(file)) return null;

        var candidate = Path.Combine(Directory.GetCurrentDirectory(), file);

        if (File.Exists(candidate))
        {
            return new LibraryPresence
            {
                LibraryName = file,
                ResolvedPath = Path.GetFullPath(candidate),
                LocationKind = LibraryLocationKind.OnRelativePath
            };
        }

        return null;
    }

    private static string? GetCandidatePath(string? baseDir, string file)
    {
        var candidate = baseDir is not null
            ? Path.Combine(baseDir, file)
            : file;

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