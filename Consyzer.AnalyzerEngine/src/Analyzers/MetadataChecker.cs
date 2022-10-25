using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Exceptions;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    /// <summary>
    /// [Static] Contains general methods for determining the presence of metadata in binary files.
    /// </summary>
    public static class MetadataChecker
    {
        /// <summary>
        /// Returns a <b>IEnumerable<FileInfo></b> collection consisting of entities representing metadata files.
        /// </summary>
        /// <param name="fileInfos"></param>
        public static IEnumerable<FileInfo> GetMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataFile(f));
        }

        /// <summary>
        /// Returns a <b>IEnumerable<FileInfo></b> collection consisting of entities that do NOT represent metadata files.
        /// </summary>
        /// <param name="fileInfos"></param>
        public static IEnumerable<FileInfo> GetNotMetadataFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataFile(f));
        }

        /// <summary>
        /// Checks whether the binary contains ECMA-355 standard metadata.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public static bool IsMetadataFile(FileInfo fileInfo)
        {
            ExceptionThrower.ThrowExceptionIfFileDoesNotExists(fileInfo);

            try
            {
                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var peReader = new PEReader(fileStream, PEStreamOptions.Default))
                    {
                        return peReader.HasMetadata;
                    }
                }
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a collection of <b>IEnumerable<FileInfo></b> entities representing metadata assembly files.
        /// </summary>
        /// <param name="fileInfos"></param>
        public static IEnumerable<FileInfo> GetMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => IsMetadataAssemblyFile(f));
        }

        /// <summary>
        /// Returns a collection of <b>IEnumerable<FileInfo></b> entities that do NOT represent metadata assembly files.
        /// </summary>
        /// <param name="fileInfos"></param>
        public static IEnumerable<FileInfo> GetNotMetadataAssemblyFiles(IEnumerable<FileInfo> fileInfos)
        {
            return fileInfos.Where(f => !IsMetadataAssemblyFile(f));
        }

        /// <summary>
        /// Checks whether the metadata file is an assembly.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public static bool IsMetadataAssemblyFile(FileInfo fileInfo)
        {
            ExceptionThrower.ThrowExceptionIfFileDoesNotExists(fileInfo);

            try
            {
                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var peReader = new PEReader(fileStream, PEStreamOptions.Default))
                    {
                        if (!peReader.HasMetadata)
                        {
                            return false;
                        }

                        var mdReader = peReader.GetMetadataReader();

                        return mdReader.IsAssembly;
                    }
                }
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }
    }
}
