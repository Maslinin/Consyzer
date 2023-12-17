using System.Collections.Generic;
using Consyzer.Checkers.Models;

namespace Consyzer.Checkers;

internal interface IFileExistenceChecker
{
    FileExistenceStatus GetMaxFileExistanceStatus(IEnumerable<string> filePaths);
    FileExistenceStatus GetMinFileExistanceStatus(string filePath);
    FileExistenceStatus CheckFileExistenceAtAnalysisPath(string filePath);
    FileExistenceStatus CheckFileExistenceAtAbsolutePath(string filePath);
    FileExistenceStatus CheckFileExistenceAtRelativePath(string filePath);
    FileExistenceStatus CheckFileExistenceAtSystemDirectory(string filePath);
}