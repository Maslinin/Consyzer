namespace Consyzer.Core.Models;

internal enum AppExitCode
{
    Success = 0,

    // Application behavior-related
    UnexpectedError = -1,
    NoAnalysisDirectory = -2,
    NoSearchPattern = -3,
    NoFilesFound = -4,
    AllFilesInvalid = -5
}
