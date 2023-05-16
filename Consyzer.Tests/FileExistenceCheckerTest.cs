using Xunit;
using System.IO;
using static Consyzer.Tests.TestConstants.FileLocation;

namespace Consyzer.Tests
{
    public sealed class FileExistenceCheckerTest
    {
        private readonly string _fileAtSystemFolder = "kernel32";

        [Fact]
        public void GetMaxFileExistanceStatus_ShouldReturnSomeShit()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);
            var paths = new string[] 
            { 
                this._fileAtSystemFolder, //system folder
                MetadataAssemblyFileInfo.Name, //relative path/Analysis path
                MetadataAssemblyLocation //Absolute Path/Analysis path
            };

            var status = fileExistenceChecker.GetMaxFileExistanceStatus(paths);

            Assert.Equal(FileExistenceStatus.FileExistsAtSystemFolder, status);
        }

        [Fact]
        public void GetMinFileExistanceStatus_ShouldReturnSomeShit()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.GetMinFileExistanceStatus(this._fileAtSystemFolder);

            Assert.Equal(FileExistenceStatus.FileExistsAtSystemFolder, status);
        }

        [Fact]
        public void CheckFileExistenceAtAnalysisPath_ShouldReturnFileExistsAtAnalysisPathCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtAnalysisPath(MetadataAssemblyLocation);

            Assert.Equal(FileExistenceStatus.FileExistsAtAnalysisPath, status);
        }

        [Fact]
        public void CheckFileExistenceAtAnalysisPath_ShouldReturnFileDoesNotExistCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtAnalysisPath(this._fileAtSystemFolder);

            Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
        }

        [Fact]
        public void CheckFileExistenceAtAbsolutePath_ShouldReturnFileExistsAtAbsolutePathCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtAbsolutePath(MetadataAssemblyLocation);

            Assert.Equal(FileExistenceStatus.FileExistsAtAbsolutePath, status);
        }

        [Fact]
        public void CheckFileExistenceAtAbsolutePath_ShouldReturnFileDoesNotExistCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtAbsolutePath(this._fileAtSystemFolder);

            Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
        }

        [Fact]
        public void CheckFileExistenceAtRelativePath_ShouldReturnFileExistsAtRelativePathCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtRelativePath(Path.Combine("\\", MetadataAssemblyFileInfo.Name));

            Assert.Equal(FileExistenceStatus.FileExistsAtRelativePath, status);
        }

        [Fact]
        public void CheckFileExistenceAtRelativePath_ShouldReturnFileDoesNotExistCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtRelativePath(this._fileAtSystemFolder);

            Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
        }

        [Fact]
        public void CheckFileExistenceAtSystemFolder_ShouldReturnFileExistsAtSystemFoldeCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtSystemFolder(this._fileAtSystemFolder);

            Assert.Equal(FileExistenceStatus.FileExistsAtSystemFolder, status);
        }

        [Fact]
        public void CheckFileExistenceAtSystemFolder_ShouldReturnFileDoesNotExistCode()
        {
            var fileExistenceChecker = new FileExistenceChecker(MetadataAssemblyFileInfo.DirectoryName);

            var status = fileExistenceChecker.CheckFileExistenceAtSystemFolder(MetadataAssemblyFileInfo.Name);

            Assert.Equal(FileExistenceStatus.FileDoesNotExist, status);
        }
    }
}
