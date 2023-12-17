using System.IO;
using System.Collections.Generic;

namespace Consyzer.Filters;

internal interface IMetadataFileFilter
{
    IEnumerable<FileInfo> GetMetadataFiles(IEnumerable<FileInfo> fileInfos);
    IEnumerable<FileInfo> GetNonMetadataFiles(IEnumerable<FileInfo> fileInfos);
    IEnumerable<FileInfo> GetMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos);
    IEnumerable<FileInfo> GetNonMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos);
}