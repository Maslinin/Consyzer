using Xunit;
using System.IO;
using System.Collections.Generic;
using Consyzer.Filters;
using static Consyzer.Tests.TestConstants;

namespace Consyzer.Tests.Filters;

public sealed class EcmaMetadataFileFilterTest
{
    private readonly IMetadataFileFilter _fileFilter;
    private readonly IEnumerable<FileInfo> _fileInfosForTest;

    public EcmaMetadataFileFilterTest()
    {
        this._fileFilter = new EcmaMetadataFileFilter();
        this._fileInfosForTest = new List<FileInfo>
        {
            MetadataAssemblyFileInfo,
            NotMetadataAssemblyFileInfo
        };
    }

    [Fact]
    public void GetMetadataFiles_ShouldReturnOnlyMetadataFiles()
    {
        var metadataFiles = this._fileFilter.GetMetadataFiles(this._fileInfosForTest);

        Assert.DoesNotContain(NotMetadataAssemblyFileInfo, metadataFiles);
    }

    [Fact]
    public void GetNonMetadataFiles_ShouldReturnOnlyNonMetadataFiles()
    {
        var nonMetadataFiles = this._fileFilter.GetNonMetadataFiles(this._fileInfosForTest);

        Assert.DoesNotContain(MetadataAssemblyFileInfo, nonMetadataFiles);
    }

    [Fact]
    public void GetMetadataAssemblyFiles_ShouldReturnOnlyMetadataAssemblyFiles()
    {
        var metadataAssemblyFiles = this._fileFilter.GetMetadataAssemblyFiles(this._fileInfosForTest);

        Assert.DoesNotContain(NotMetadataAssemblyFileInfo, metadataAssemblyFiles);
    }

    [Fact]
    public void GetNonMetadataAssemblyFiles_ShouldReturnOnlyNonMetadataAssemblyFiles()
    {
        var nonMetadataAssemblyFiles = this._fileFilter.GetNonMetadataAssemblyFiles(this._fileInfosForTest);

        Assert.DoesNotContain(MetadataAssemblyFileInfo, nonMetadataAssemblyFiles);
    }
}