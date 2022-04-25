using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Helpers
{
    /// <summary>
    /// [Static] Contains auxiliary methods for interacting with the IO system.
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// Returns a collection of <b>BinaryFileInfo</b> instances consisting of binary files with specified extensions from the specified path.
        /// </summary>
        /// <param name="pathToBinaries"></param>
        /// <param name="binaryExtensions"></param>
        /// <returns><b>BinaryFileInfo</b> Instance Collection</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public static IEnumerable<BinaryFileInfo> GetBinaryFilesInfoFrom(string pathToBinaries, IEnumerable<string> binaryExtensions = null)
        {
            if (!Directory.Exists(pathToBinaries))
            {
                throw new DirectoryNotFoundException("The directory specified for the analysis does not exist.");
            }
            if (binaryExtensions is null)
            {
                binaryExtensions = new List<string> { ".exe", ".dll" };
            }

            try
            {
                var dirInfo = new DirectoryInfo(pathToBinaries).GetFiles().Where(f => binaryExtensions.Contains(f.Extension));

                return dirInfo.Select(i => new BinaryFileInfo(i.FullName));
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException($"You do not have read permissions in the specified folder: {pathToBinaries}.", e);
            }
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the path is absolute or relative.
        /// </summary>
        /// <param name="path"></param>
        /// <returns><b>true</b> if the path is absolute or relative; otherwise <b>false</b>.</returns>
        public static bool IsAbsolutePath(string path)
        {
            return Path.IsPathRooted(path);
        }
    }
}
