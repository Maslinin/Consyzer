using Consyzer.Cryptography;

namespace Consyzer.Logging;

internal static class FileMetadataLogMessageFormatter
{
    public static string GetBaseFileInfoLog(IEnumerable<FileInfo> fileInfos)
    {
        return string.Join(Environment.NewLine, fileInfos.Select((f, i) =>
        {
            string fileInfo = $"[{i}]File: '{f.Name}':";
            string creationTime = $"\tCreation Time: '{f.CreationTime}'";

            return $"{fileInfo}{Environment.NewLine}{creationTime}";
        }));
    }

    public static string GetBaseAndHashFileInfoLog(IEnumerable<FileInfo> fileInfos, IFileHasher hasher)
    {
        return string.Join(Environment.NewLine, fileInfos.Select((f, i) =>
        {
            string fileInfo = $"[{i}]File: '{f.Name}':";
            string creationTime = $"\tCreation Time: '{f.CreationTime}'";
            string hash = $"\tSHA256 Hash Sum: '{hasher.CalculateHash(f)}'.";

            return $"{fileInfo}{Environment.NewLine}{creationTime}, {Environment.NewLine}{hash}";
        }));
    }
}