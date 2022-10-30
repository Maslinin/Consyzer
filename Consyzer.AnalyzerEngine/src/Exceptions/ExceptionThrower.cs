using System;
using System.IO;
using Consyzer.AnalyzerEngine.Analyzers;

namespace Consyzer.AnalyzerEngine.Exceptions
{
    internal static class ExceptionThrower
    {
        public static void ThrowExceptionIfFileDoesNotExists(FileInfo fileInfo)
        {
            if (fileInfo is null)
            {
                throw new ArgumentException($"{nameof(fileInfo)} is null.");
            }
        }

        public static void ThrowExceptionIfFileIsNotMetadataAssembly(FileInfo fileInfo)
        {
            ThrowExceptionIfFileDoesNotContainMetadata(fileInfo);
            if (!MetadataFilter.IsMetadataAssemblyFile(fileInfo))
            {
                throw new AssemblyFileNotSupportedException($"{fileInfo.FullName} is contains metadata, but is not an assembly.");
            }
        }

        public static void ThrowExceptionIfFileDoesNotContainMetadata(FileInfo fileInfo)
        {
            if (!MetadataFilter.IsMetadataFile(fileInfo))
            {
                throw new MetadataFileNotSupportedException($"{fileInfo.FullName} is does not contain metadata.");
            }
        }
    }
}
