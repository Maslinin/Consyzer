using System;
using System.IO;
using Consyzer.File;
using Consyzer.Metadata;

namespace Consyzer
{
    internal static class ExceptionChecker
    {
        public static void ThrowExceptionIfFileDoesNotExist(FileInfo fileInfo)
        {
            if (fileInfo is null)
            {
                throw new ArgumentException($"{nameof(fileInfo)} is null.");
            }
        }

        public static void ThrowExceptionIfFileIsNotMetadataAssembly(FileInfo fileInfo)
        {
            ThrowExceptionIfFileDoesNotContainMetadata(fileInfo);
            if (!MetadataFileFilter.IsMetadataAssemblyFile(fileInfo))
            {
                throw new FileNotSupportedException($"{fileInfo.FullName} contains metadata, but is not an assembly.");
            }
        }

        public static void ThrowExceptionIfFileDoesNotContainMetadata(FileInfo fileInfo)
        {
            if (!MetadataFileFilter.IsMetadataFile(fileInfo))
            {
                throw new FileNotSupportedException($"{fileInfo.FullName} does not contain metadata.");
            }
        }
    }
}
