using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;

namespace Consyzer.Analyzer
{
    public class DynamicAnalyzer
    {
        public static IEnumerable<FileInfo> GetBinaryFilesName(string pathToBinaryFiles, string[] filesExtensions)
        {
            if(!Directory.Exists(pathToBinaryFiles))
            {
                throw new DirectoryNotFoundException("The directory specified for the analysis does not exist");
            }
            if(filesExtensions is null)
            {
                throw new ArgumentNullException("The \"filesExtensions\" argument: there is no reference to the object");
            }

            try
            {
                var dirInfo = new DirectoryInfo(pathToBinaryFiles);

                IEnumerable<FileInfo> binaryFiles = dirInfo.EnumerateFiles();
                return binaryFiles.Where(f => filesExtensions.Contains(f.Extension));
            }
            catch(Exception e)
            {
                throw new UnauthorizedAccessException($"You do not have read permissions in the specified folder: {pathToBinaryFiles}", e);
            }
        }

        public static SyntaxTree GetSyntaxTreeOfBinary(string pathToBinary)
        {
            return new CSharpDecompiler(pathToBinary, new DecompilerSettings()).DecompileModuleAndAssemblyAttributes();
        }
    }
}
