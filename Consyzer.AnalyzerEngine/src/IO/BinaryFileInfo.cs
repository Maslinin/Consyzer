using System;
using System.IO;
using Consyzer.AnalyzerEngine.Analyzers;
using Consyzer.AnalyzerEngine.Cryptography;

namespace Consyzer.AnalyzerEngine.IO
{
    /// <summary>
    /// [Sealed] Provides detailed information about the binary file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class BinaryFileInfo
    {
        /// <summary>
        /// Gets a <b>FileInfo</b> instance of the current file containing basic information about the file.
        /// </summary>
        public FileInfo BaseFileInfo { get; }
        /// <summary>
        /// Gets true if the binary file contains metadata; otherwise false.
        /// </summary>
        public bool HasMetadata { get; }
        /// <summary>
        /// Gets true if the binary is a metadata assembly; otherwise false.
        /// </summary>
        public bool IsMetadataAssembly { get; }
        /// <summary>
        /// Gets an instance of <b>HashFileInfo</b> containing information about the hash amounts of the current file.
        /// </summary>
        public FileHashInfo HashInfo { get; }

        /// <summary>
        /// Initializes a new instance of <b>BinaryFileInfo</b>.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public BinaryFileInfo(string pathToBinary)
        {
            if (pathToBinary is null)
            {
                throw new ArgumentException($"{nameof(pathToBinary)} is null.");
            }
            if (!File.Exists(pathToBinary))
            {
                throw new FileNotFoundException($"File {pathToBinary} does not exist.");
            }

            var info = new FileInfo(pathToBinary);

            BaseFileInfo = info;
            HasMetadata = CommonAnalyzersHelper.HasMetadata(pathToBinary);
            IsMetadataAssembly = CommonAnalyzersHelper.MetadataFileIsAssembly(pathToBinary);
            HashInfo = new FileHashInfo(info);
        }

    }
}
