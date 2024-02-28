using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.Filters;

public sealed class EcmaMetadataFileFilter : IMetadataFileFilter
{
    public IEnumerable<FileInfo> GetMetadataFiles(IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Where(HasMetadata);
    }

    public IEnumerable<FileInfo> GetNonMetadataFiles(IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Where(f => !HasMetadata(f));
    }

    public IEnumerable<FileInfo> GetMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Where(IsAssembly);
    }

    public IEnumerable<FileInfo> GetNonMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
    {
        return fileInfos.Where(f => !IsAssembly(f));
    }

    private static bool HasMetadata(FileInfo fileInfo)
    {
        using var fileStream = fileInfo.OpenRead();
        using var peReader = new PEReader(fileStream);
        return peReader.HasMetadata;
    }

    private static bool IsAssembly(FileInfo fileInfo)
    {
        using var fileStream = fileInfo.OpenRead();
        using var peReader = new PEReader(fileStream);

        if (!peReader.HasMetadata) return false;

        var mdReader = peReader.GetMetadataReader();
        return mdReader.IsAssembly;
    }
}