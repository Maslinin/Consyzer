namespace Consyzer.Core.Resources;

internal sealed class FileStreamAccessor : IResourceAccessor<FileInfo, Stream>
{
    private readonly Dictionary<string, Stream> _resources = [];

    public Stream Get(FileInfo file)
    {
        string filePath = file.FullName;

        if (this._resources.TryGetValue(file.FullName, out var resource))
        {
            return resource;
        }

        var stream = file.OpenRead();
        this._resources[filePath] = stream;

        return stream;
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
