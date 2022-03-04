using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.Metadata;

namespace Consyzer.Analyzer
{
    public class DynamicAnalyzer
    {
        public static SyntaxTree GetSyntaxTreeOfBinary(string pathToBinary)
        {
            try
            {
                return new CSharpDecompiler(pathToBinary, new DecompilerSettings()).DecompileWholeModuleAsSingleFile();
            }
            catch(PEFileNotSupportedException e)
            {
                throw new PEFileNotSupportedException("This binary file does not contain metadata.", e);
            }
        }
        public static SyntaxTree GetStringOfBinary(string pathToBinary)
        {
            try
            {
                return new CSharpDecompiler(pathToBinary, new DecompilerSettings()).DecompileWholeModuleAsSingleFile();
            }
            catch (PEFileNotSupportedException e)
            {
                throw new PEFileNotSupportedException("This binary file does not contain metadata.", e);
            }
        }
    }
}
