using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.FileInteraction
{
    public static class MetadataFileFilter
    {
        public static IEnumerable<FileInfo> GetMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataFile(f, false));
        }

        public static IEnumerable<FileInfo> GetNotMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataFile(f, false));
        }

        public static IEnumerable<FileInfo> GetMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataFile(f, true));
        }

        public static IEnumerable<FileInfo> GetNotMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataFile(f, true));
        }

        public static bool IsMetadataFile(FileInfo fileInfo, bool checkAssembly)
        {
            try
            {
                using var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
                using var peReader = new PEReader(fileStream, PEStreamOptions.Default);
                if (!peReader.HasMetadata) return false;

                var mdReader = peReader.GetMetadataReader();
                if (checkAssembly) return mdReader.IsAssembly;

                return true;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }
    }
}
