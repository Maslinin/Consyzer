using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;
using Consyzer.Core.Cryptography;

namespace Consyzer.Core.Extractors;

internal sealed class AssemblyMetadataExtractor(
    IFileHasher hasher,
    IResourceAccessor<FileInfo, PEReader> peReaderManager
) : IExtractor<FileInfo, AssemblyMetadata>
{
    public AssemblyMetadata Extract(FileInfo file)
    {
        return new AssemblyMetadata
        {
            File = file,
            Version = ExtractVersion(file),
            CreationDateUtc = ExtractCreationDate(file),
            Sha256 = ExtractHash(file)
        };
    }

    private string ExtractVersion(FileInfo file)
    {
        var peReader = peReaderManager.Get(file);

        var mdReader = peReader.GetMetadataReader();
        return mdReader.GetAssemblyDefinition().Version?.ToString() ?? "unknown";
    }

    private string ExtractHash(FileInfo file)
    {
        return hasher.CalculateHash(file);
    }

    private static DateTime ExtractCreationDate(FileInfo file)
    {
        return file.CreationTimeUtc;
    }
}