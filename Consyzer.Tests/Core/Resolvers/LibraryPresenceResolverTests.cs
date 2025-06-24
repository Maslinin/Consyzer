using Xunit;
using Consyzer.Core.Models;
using Consyzer.Core.Resolvers;

namespace Consyzer.Tests.Core.Resolvers;

public sealed class LibraryPresenceResolverTests : IDisposable
{
    private const string DummyFileContent = "dummy";
    private const string DllExtension = ".dll";
    private const string TestLibAnalyzed = "testlib-analyzed" + DllExtension;
    private const string TestLibAbs = "testlib-abs" + DllExtension;
    private const string TestLibRel = "testlib-rel" + DllExtension;
    private const string TestLibEnv = "testlib-env" + DllExtension;
    private const string TestLibMissing = "nonexistent-lib-123456" + DllExtension;

    private static readonly string PathVariableName = "PATH";

    private readonly string _analyzedDirectory = Path.Combine(Path.GetTempPath(), "analyzed-" + Guid.NewGuid());
    private readonly string _envPathDirectory = Path.Combine(Path.GetTempPath(), "envpath-" + Guid.NewGuid());

    public LibraryPresenceResolverTests()
    {
        Directory.CreateDirectory(this._analyzedDirectory);
        Directory.CreateDirectory(this._envPathDirectory);
    }

    [Fact]
    public void Resolve_ShouldReturnInAnalyzedDirectory_WhenLibraryIsInAnalyzedDir()
    {
        var libPath = Path.Combine(this._analyzedDirectory, TestLibAnalyzed);
        File.WriteAllText(libPath, DummyFileContent);

        try
        {
            var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
            var result = resolver.Resolve(TestLibAnalyzed);

            Assert.Equal(LibraryLocationKind.InAnalyzedDirectory, result.LocationKind);
            Assert.Equal(libPath, result.ResolvedPath);
        }
        finally
        {
            File.Delete(libPath);
        }
    }

    [Fact]
    public void Resolve_ShouldReturnOnAbsolutePath_WhenLibraryExistsAtAbsolutePath()
    {
        var libPath = Path.Combine(Path.GetTempPath(), TestLibAbs);
        File.WriteAllText(libPath, DummyFileContent);

        try
        {
            var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
            var result = resolver.Resolve(libPath);

            Assert.Equal(LibraryLocationKind.OnAbsolutePath, result.LocationKind);
            Assert.Equal(libPath, result.ResolvedPath);
        }
        finally
        {
            File.Delete(libPath);
        }
    }

    [Fact]
    public void Resolve_ShouldReturnOnRelativePath_WhenLibraryExistsInCwd()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), TestLibRel);
        File.WriteAllText(filePath, DummyFileContent);

        try
        {
            var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
            var result = resolver.Resolve(TestLibRel);

            Assert.Equal(LibraryLocationKind.OnRelativePath, result.LocationKind);
            Assert.Equal(filePath, result.ResolvedPath);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void Resolve_ShouldReturnInEnvironmentPath_WhenLibraryInPath()
    {
        var libPath = Path.Combine(this._envPathDirectory, TestLibEnv);
        File.WriteAllText(libPath, DummyFileContent);

        var originalPath = Environment.GetEnvironmentVariable(PathVariableName) ?? string.Empty;
        Environment.SetEnvironmentVariable(PathVariableName, this._envPathDirectory + Path.PathSeparator + originalPath);

        try
        {
            var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
            var result = resolver.Resolve(TestLibEnv);

            Assert.Equal(LibraryLocationKind.InEnvironmentPath, result.LocationKind);
            Assert.Equal(Path.GetFullPath(libPath), result.ResolvedPath);
        }
        finally
        {
            Environment.SetEnvironmentVariable(PathVariableName, originalPath);
            File.Delete(libPath);
        }
    }

    [Fact]
    public void Resolve_ShouldReturnInSystemDirectory_WhenLibraryIsInSystemDirectory()
    {
        var systemDir = Environment.SystemDirectory;
        var libName = Directory.GetFiles(systemDir)
            .Select(Path.GetFileName)
            .FirstOrDefault(name => name?.EndsWith(DllExtension, StringComparison.OrdinalIgnoreCase) is true);

        if (libName is null) return;

        var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
        var result = resolver.Resolve(libName);

        Assert.Contains(result.LocationKind, new[] {
            LibraryLocationKind.InSystemDirectory,
            LibraryLocationKind.InEnvironmentPath
        });
        Assert.Contains(systemDir, result.ResolvedPath!);
    }

    [Fact]
    public void Resolve_ShouldReturnMissing_WhenLibraryNotFound()
    {
        var resolver = new CrossPlatformLibraryPresenceResolver(this._analyzedDirectory);
        var result = resolver.Resolve(TestLibMissing);

        Assert.Equal(LibraryLocationKind.Missing, result.LocationKind);
        Assert.Null(result.ResolvedPath);
    }

    public void Dispose()
    {
        DeleteIfExists(this._analyzedDirectory);
        DeleteIfExists(this._envPathDirectory);
    }

    private static void DeleteIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
}
