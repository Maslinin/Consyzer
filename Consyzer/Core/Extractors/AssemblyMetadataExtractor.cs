using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;
using Consyzer.Core.Cryptography;

namespace Consyzer.Core.Extractors;

internal sealed class AssemblyMetadataExtractor(
    IFileHasher hasher,
    IResourceAccessor<FileInfo, PEReader> peReaderAccessor
) : IExtractor<FileInfo, AssemblyMetadata>
{
    public AssemblyMetadata Extract(FileInfo file)
    {
        return new AssemblyMetadata
        {
            File = file,
            Version = GetVersion(file),
            CreationDateUtc = GetCreationDate(file),
            Sha256 = GetHash(file)
        };
    }

    private string GetVersion(FileInfo file)
    {
        var peReader = peReaderAccessor.Get(file);

        var mdReader = peReader.GetMetadataReader();
        return mdReader.GetAssemblyDefinition().Version?.ToString() ?? "unknown";
    }

    private string GetHash(FileInfo file)
    {
        return hasher.CalculateHash(file);
    }

    private static DateTime GetCreationDate(FileInfo file)
    {
        return file.CreationTimeUtc;
    }
}