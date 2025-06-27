namespace Consyzer.Core.Resources;

internal sealed class FileStreamAccessor : IResourceAccessor<FileInfo, Stream>
{
    private readonly Dictionary<string, Stream> _resources = [];

    public Stream Get(FileInfo file)
    {
        string filePath = file.FullName;

        if (_resources.TryGetValue(file.FullName, out var resource))
        {
            resource.Position = 0;
            return resource;
        }

        var stream = file.OpenRead();
        _resources[filePath] = stream;

        return stream;
    }

    public void Dispose()
    {
        foreach (var resource in _resources.Values)
        {
            resource.Dispose();
        }

        _resources.Clear();
    }
}