using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Decoder;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Analyzer
{
    public static class DynamicAnalyzer
    {
        public static IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo(string pathToBinary)
        {
            var dllImports = new List<ImportedMethodInfo>();

            using (var fileStream = new FileStream(pathToBinary, FileMode.Open, FileAccess.Read))
            {
                using (var peReader = new PEReader(fileStream))
                {
                    var mdReader = peReader.GetMetadataReader();

                    foreach (var typeDef in mdReader.TypeDefinitions.Select(t => mdReader.GetTypeDefinition(t)))
                    {
                        foreach (var methodDef in typeDef.GetMethods().Select(m => mdReader.GetMethodDefinition(m)))
                        {
                            var import = methodDef.GetImport();
                            if (import.Name.IsNil)
                            {
                                continue;
                            }

                            var signature = new SignatureDecoder(mdReader).GetDecodedSignature(methodDef);

                            dllImports.Add(new ImportedMethodInfo(signature, mdReader.GetString(mdReader.GetModuleReference(import.Module).Name), import.Attributes.ToString()));
                        }
                    }
                }
            }
            
            return dllImports;
        }
    }
}
