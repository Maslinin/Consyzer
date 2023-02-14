using System.IO;
using System.Collections.Generic;

namespace Consyzer.File
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.FullName == y.FullName;
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
