using System.Reflection.PortableExecutable;

namespace Consyzer.Core.Resources;

internal sealed class PEReaderAccessor(
    IResourceAccessor<FileInfo, Stream> fileStreamAccessor
) : IResourceAccessor<FileInfo, PEReader>
{
    private readonly Dictionary<string, PEReader> _resources = [];

    public PEReader Get(FileInfo file)
    {
        string filePath = file.FullName;

        if (this._resources.TryGetValue(filePath, out var resource))
        {
            return resource;
        }

        var stream = fileStreamAccessor.Get(file);
        var reader = new PEReader(stream);

        this._resources[filePath] = reader;

        return reader;
    }

    public void Dispose()
    {
        foreach (var resource in this._resources.Values)
        {
            resource.Dispose();
        }

        this._resources.Clear();
    }
}