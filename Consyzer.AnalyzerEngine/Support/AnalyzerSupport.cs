using System.IO;
using System.Linq;
using System.Collections.Generic;

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
                new CSharpDecompiler(pathToBinary, new ICSharpCode.Decompiler.DecompilerSettings()).DecompileModuleAndAssemblyAttributes();
                return true;
            }
            catch(PEFileNotSupportedException)
            {
                return false;
            }
        }

        public static IEnumerable<FileInfo> GetManagedFilesFromList(IEnumerable<FileInfo> binaryFiles)
        {
            return binaryFiles.Where(f => AnalyzerSupport.IsManaged(f.FullName));
        }
    }
}
