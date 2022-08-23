using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.IO;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    /// <summary>
    /// [Static] Contains general methods to facilitate interaction with the metadata analyzer.
    /// </summary>
    public static class CommonAnalyzersHelper
    {
        /// <summary>
        /// Checks whether the binary contains ECMA-355 standard metadata.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PEFileNotSupportedException"></exception>
        public static bool HasMetadata(string pathToBinary)
        {
            try
            {
                return new PEReader(new FileStream(pathToBinary, FileMode.Open, FileAccess.Read), PEStreamOptions.Default).HasMetadata;
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

        #region GetFilesContainsMetadata overloads

        /// <summary>
        /// Returns a <b>FileInfo</b> collection consisting of entities representing metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<FileInfo> GetFilesContainsMetadata(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => HasMetadata(f.FullName));
        }

        /// <summary>
        /// Returns a <b>PEReader</b> collection consisting of entities representing metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<PEReader> GetFilesContainsMetadata(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => f.HasMetadata);
        }

        /// <summary>
        /// Returns a <b>BinaryFileInfo</b> collection consisting of entities representing metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<BinaryFileInfo> GetFilesContainsMetadata(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => f.HasMetadata);
        }

        #endregion

        #region GetFilesNotContainsMetadata overloads

        /// <summary>
        /// Returns a <b>FileInfo</b> collection consisting of entities that do NOT represent metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<FileInfo> GetFilesNotContainsMetadata(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !HasMetadata(f.FullName));
        }

        /// <summary>
        /// Returns a <b>PEReader</b> collection consisting of entities that do NOT represent metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<PEReader> GetFilesNotContainsMetadata(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => !f.HasMetadata);
        }

        /// <summary>
        /// Returns a <b>BinaryFileInfo</b> collection consisting of entities that do NOT represent metadata files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<BinaryFileInfo> GetFilesNotContainsMetadata(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !f.HasMetadata);
        }

        #endregion

        /// <summary>
        /// Checks whether the metadata file is an assembly.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PEFileNotSupportedException"></exception>
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

        #region GetMetadataAssemblyFiles overloads

        /// <summary>
        /// Returns a collection of <b>FileInfo</b> entities representing metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<FileInfo> GetMetadataAssemblyFiles(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => MetadataFileIsAssembly(f.FullName));
        }

        /// <summary>
        /// Returns a collection of <b>PEReader</b> entities representing metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<PEReader> GetMetadataAssemblyFiles(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => f.GetMetadataReader().IsAssembly);
        }

        /// <summary>
        /// Returns a collection of <b>BinaryFileInfo</b> entities representing metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<BinaryFileInfo> GetMetadataAssemblyFiles(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => f.IsMetadataAssembly);
        }

        #endregion

        #region GetNotMetadataAssemblyFiles overloads

        /// <summary>
        /// Returns a collection of <b>FileInfo</b> entities that do NOT represent metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<FileInfo> GetNotMetadataAssemblyFiles(this IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !MetadataFileIsAssembly(f.FullName));
        }

        /// <summary>
        /// Returns a collection of <b>PEReader</b> entities that do NOT represent metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<PEReader> GetNotMetadataAssemblyFiles(this IEnumerable<PEReader> binaryFiles)
        {
            return binaryFiles.Where(f => !f.GetMetadataReader().IsAssembly);
        }

        /// <summary>
        /// Returns a collection of <b>BinaryFileInfo</b> entities that do NOT represent metadata assembly files.
        /// </summary>
        /// <param name="binaryFiles"></param>
        public static IEnumerable<BinaryFileInfo> GetNotMetadataAssemblyFiles(this IEnumerable<BinaryFileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !f.IsMetadataAssembly);
        }

        #endregion

    }
}
