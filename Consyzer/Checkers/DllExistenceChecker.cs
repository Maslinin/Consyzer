using Microsoft.Extensions.Options;
using Consyzer.Options.Models;
using Consyzer.Checkers.Models;

namespace Consyzer.Checkers;

internal sealed class DllExistenceChecker : IFileExistenceChecker
{
    private const string DllExtension = ".dll";

    private readonly string _analysisDirectory;

    public DllExistenceChecker(IOptions<CommandLineOptions> options)
    {
        this._analysisDirectory = options.Value.AnalysisDirectory;
    }

    public FileExistenceStatus GetMaxFileExistanceStatus(IEnumerable<string> filePaths)
    {
        return filePaths.Max(this.GetMinFileExistanceStatus);
    }

    public FileExistenceStatus GetMinFileExistanceStatus(string filePath)
    {
        var statuses = new[]
        {
            this.CheckFileExistenceAtAnalysisPath(filePath),
            this.CheckFileExistenceAtAbsolutePath(filePath),
            this.CheckFileExistenceAtRelativePath(filePath),
            this.CheckFileExistenceAtSystemDirectory(filePath)
        };

        return statuses.Min();
    }

    public FileExistenceStatus CheckFileExistenceAtAnalysisPath(string filePath)
    {
        string correctPath = EnsureCorrectFilePath(filePath, this._analysisDirectory);
        return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtAnalysisPath : FileExistenceStatus.FileDoesNotExist;
    }

    public FileExistenceStatus CheckFileExistenceAtAbsolutePath(string filePath)
    {
        string correctPath = EnsureDllExtension(filePath);
        return Path.IsPathFullyQualified(correctPath) ? FileExistenceStatus.FileExistsAtAbsolutePath : FileExistenceStatus.FileDoesNotExist;
    }

    public FileExistenceStatus CheckFileExistenceAtRelativePath(string filePath)
    {
        string correctPath = EnsureDllExtension(filePath);
        return Path.IsPathRooted(correctPath) ? FileExistenceStatus.FileExistsAtRelativePath : FileExistenceStatus.FileDoesNotExist;
    }

    public FileExistenceStatus CheckFileExistenceAtSystemDirectory(string filePath)
    {
        string correctPath = EnsureCorrectFilePath(filePath, Environment.SystemDirectory);
        return File.Exists(correctPath) ? FileExistenceStatus.FileExistsAtSystemDirectory : FileExistenceStatus.FileDoesNotExist;
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static string EnsureCorrectFilePath(string filePath, string directory)
    {
        string correctPath = EnsureAbsoluteFilePath(directory, filePath);
        return EnsureDllExtension(correctPath);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static string EnsureDllExtension(string filePath)
    {
        return Path.HasExtension(filePath) ? filePath : Path.ChangeExtension(filePath, DllExtension);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static string EnsureAbsoluteFilePath(string directory, string filePath)
    {
        return Path.IsPathFullyQualified(filePath) ? filePath : Path.Combine(directory, filePath);
    }
}