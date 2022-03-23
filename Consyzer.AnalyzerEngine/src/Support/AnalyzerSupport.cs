using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.CommonModels.FileInfoModels;

namespace Consyzer.AnalyzerEngine.Support
{
    public static class AnalyzerSupport
    {
        public static bool HasMetadata(string pathToBinary)
        {
            try
            {
                return new PEReader(new FileStream(pathToBinary, FileMode.Open, FileAccess.Read), PEStreamOptions.Default).HasMetadata;
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("The binary file could not be loaded due to its protection level.", ex);
            }
            catch(BadImageFormatException ex)
            {
                throw new PEFileNotSupportedException("File is not a PE file.", ex);
            }
        }

        #region HasMetadata: IEnumerable extensions

        public static IEnumerable<FileInfo> GetFilesContainsMetadata(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => AnalyzerSupport.HasMetadata(f.FullName));
        }

        public static IEnumerable<PEReader> GetFilesContainsMetadata(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => f.HasMetadata);
        }

        public static IEnumerable<BinaryFileInfo> GetFilesContainsMetadata(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => f.HasMetadata);
        }

        public static IEnumerable<FileInfo> GetFilesNotContainsMetadata(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !AnalyzerSupport.HasMetadata(f.FullName));
        }

        public static IEnumerable<PEReader> GetFilesNotContainsMetadata(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => !f.HasMetadata);
        }

        public static IEnumerable<BinaryFileInfo> GetFilesNotContainsMetadata(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !f.HasMetadata);
        }

        #endregion

        public static bool MetadataFileIsAssembly(string pathToBinary)
        {
            try
            {
                var reader = new PEReader(new FileStream(pathToBinary, FileMode.Open, FileAccess.Read), PEStreamOptions.Default);
                if (!reader.HasMetadata)
                {
                    return false;
                }

                return reader.GetMetadataReader().IsAssembly;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("The binary file could not be loaded due to its protection level.", ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new PEFileNotSupportedException("File is not a PE file.", ex);
            }
        }

        #region IsAssembly: IEnumerable extensions

        public static IEnumerable<FileInfo> GetAssemblyFiles(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => AnalyzerSupport.MetadataFileIsAssembly(f.FullName));
        }

        public static IEnumerable<PEReader> GetAssemblyFiles(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => f.GetMetadataReader().IsAssembly);
        }

        public static IEnumerable<BinaryFileInfo> GetAssemblyFiles(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => f.IsAssembly);
        }

        public static IEnumerable<FileInfo> GetNotAssemblyFiles(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !AnalyzerSupport.MetadataFileIsAssembly(f.FullName));
        }

        public static IEnumerable<PEReader> GetNotAssemblyFiles(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => !f.GetMetadataReader().IsAssembly);
        }

        public static IEnumerable<BinaryFileInfo> GetNotAssemblyFiles(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !f.IsAssembly);
        }

        #endregion
    }
}
