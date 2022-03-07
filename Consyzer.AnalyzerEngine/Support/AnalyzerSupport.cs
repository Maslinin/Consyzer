using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;

namespace Consyzer.AnalyzerEngine.Support
{
    public static class AnalyzerSupport
    {
        public static bool IsManaged(string pathToBinary)
        {
            try
            {
                return new PEReader(new FileStream(pathToBinary, FileMode.Open, FileAccess.Read), PEStreamOptions.Default).HasMetadata;
            }
            catch(UnauthorizedAccessException e)
            {
                throw new UnauthorizedAccessException("The binary file could not be loaded due to its protection level", e);
            }
        }

        public static IEnumerable<FileInfo> GetManagedFiles(IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => AnalyzerSupport.IsManaged(f.FullName));
        }

        public static IEnumerable<FileInfo> GetUnManagedFiles(IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => !AnalyzerSupport.IsManaged(f.FullName));
        }
    }
}
