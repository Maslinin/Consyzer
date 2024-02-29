using Xunit;
using Consyzer.Options;
using Consyzer.Checkers;
using Consyzer.Checkers.Models;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Checkers;

public sealed class DllExistenceCheckerTest
{
    private const string FileAtSystemFolder = "kernel32";
    private readonly DllExistenceChecker _fileExistenceChecker;

    public DllExistenceCheckerTest()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new CommandLineOptions
        {
            AnalysisDirectory = MetadataAssemblyFileInfo.DirectoryName
        });

        this._fileExistenceChecker = new DllExistenceChecker(options);
    }

    [Fact]
    public void GetMaxFileExistanceStatus_ShouldReturnMaximumExistenceStatus()
    {
        var paths = new string[]
        {
                FileAtSystemFolder,
                MetadataAssemblyFileInfo.Name, //relative path [analysis path]
                MetadataAssemblyLocation //Absolute Path [analysis path]
        };

        var status = this._fileExistenceChecker.GetMaxFileExistanceStatus(paths);

        Assert.Equal(FileExistenceStatus.FileExistsAtSystemDirectory, status);
    }

    [Fact]
    public void GetMinFileExistanceStatus_ShouldReturnMinimumExistenceStatus()
    {
        var status = this._fileExistenceChecker.GetMinFileExistanceStatus(FileAtSystemFolder);

        Assert.Equal(FileExistenceStatus.FileExistsAtSystemDirectory, status);
    }

    [Fact]
    public void CheckFileExistenceAtAnalysisPath_ShouldReturnFileExistsAtAnalysisPathStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtAnalysisPath(MetadataAssemblyLocation);

        Assert.Equal(FileExistenceStatus.FileExistsAtAnalysisPath, status);
    }

    [Fact]
    public void CheckFileExistenceAtAnalysisPath_ShouldReturnFileDoesNotExistStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtAnalysisPath(FileAtSystemFolder);

        Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
    }

    [Fact]
    public void CheckFileExistenceAtAbsolutePath_ShouldReturnFileExistsAtAbsolutePathStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtAbsolutePath(MetadataAssemblyLocation);

        Assert.Equal(FileExistenceStatus.FileExistsAtAbsolutePath, status);
    }

    [Fact]
    public void CheckFileExistenceAtAbsolutePath_ShouldReturnFileDoesNotExistStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtAbsolutePath(FileAtSystemFolder);

        Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
    }

    [Fact]
    public void CheckFileExistenceAtRelativePath_ShouldReturnFileExistsAtRelativePathStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtRelativePath(Path.Combine("\\", MetadataAssemblyFileInfo.Name));

        Assert.Equal(FileExistenceStatus.FileExistsAtRelativePath, status);
    }

    [Fact]
    public void CheckFileExistenceAtRelativePath_ShouldReturnFileDoesNotExistStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtRelativePath(FileAtSystemFolder);

        Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
    }

    [Fact]
    public void CheckFileExistenceAtSystemFolder_ShouldReturnFileExistsAtSystemDirectoryStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtSystemDirectory(FileAtSystemFolder);

        Assert.Equal(FileExistenceStatus.FileExistsAtSystemDirectory, status);
    }

    [Fact]
    public void CheckFileExistenceAtSystemFolder_ShouldReturnFileDoesNotExistStatus()
    {
        var status = this._fileExistenceChecker.CheckFileExistenceAtSystemDirectory(MetadataAssemblyFileInfo.Name);

        Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
    }
}