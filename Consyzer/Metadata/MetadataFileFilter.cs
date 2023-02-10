using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.Metadata
{
    public static class MetadataFileFilter
    {
        public static IEnumerable<FileInfo> GetMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataFile(f));
        }

        public static IEnumerable<FileInfo> GetNotMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataFile(f));
        }

        public static bool IsMetadataFile(FileInfo fileInfo)
        {
            ExceptionChecker.ThrowExceptionIfFileDoesNotExist(fileInfo);

            try
            {
                using var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
                using var peReader = new PEReader(fileStream, PEStreamOptions.Default);
                return peReader.HasMetadata;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }

        public static IEnumerable<FileInfo> GetMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataAssemblyFile(f));
        }

        public static IEnumerable<FileInfo> GetNotMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataAssemblyFile(f));
        }

        public static bool IsMetadataAssemblyFile(FileInfo fileInfo)
        {
            ExceptionChecker.ThrowExceptionIfFileDoesNotExist(fileInfo);

            try
            {
                using var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
                using var peReader = new PEReader(fileStream, PEStreamOptions.Default);
                if (!peReader.HasMetadata) return false;

                var mdReader = peReader.GetMetadataReader();
                return mdReader.IsAssembly;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }
    }
}
