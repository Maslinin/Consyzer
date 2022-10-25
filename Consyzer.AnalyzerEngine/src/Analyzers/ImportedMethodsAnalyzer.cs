using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.AnalyzerEngine.Decoders;
using Consyzer.AnalyzerEngine.Analyzers.Models;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    /// <summary>
    /// Provides tools for analyzing imported methods in CLI metadata.
    /// </summary>
    public class ImportedMethodsAnalyzer : MetadataAnalyzer
    {
        public ImportedMethodsAnalyzer(FileInfo fileInfo) : base(fileInfo) { }

        /// <summary>
        /// Returns a collection <b>ImportedMethodInfo</b> containing detailed information about all methods in the assembly.
        /// </summary>
        public IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo()
        {
            var dllImports = new List<ImportedMethodInfo>();

            foreach (var methodDef in this.GetImportedMethodsDefinitions())
            {
                var importedMethodInfo = this.GetImportedMethodInfo(methodDef);
                dllImports.Add(importedMethodInfo);
            }

            return dllImports;
        }

        /// <summary>
        /// Returns a list of methods definitions imported from other assemblies.
        /// </summary>
        public IEnumerable<MethodDefinition> GetImportedMethodsDefinitions()
        {
            var importedMethods = new List<MethodDefinition>();

            foreach (var methodDef in base.GetMethodsDefinitions())
            {
                if (this.IsImportedMethod(methodDef))
                {
                    importedMethods.Add(methodDef);
                }
            }

            return importedMethods;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private bool IsImportedMethod(MethodDefinition methodDef)
        {
            var import = methodDef.GetImport();
            return !import.Name.IsNil && !import.Module.IsNil;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private ImportedMethodInfo GetImportedMethodInfo(MethodDefinition methodDef)
        {
            var signatureDecoder = new SignatureDecoder(base._mdReader);
            var signatureInfo = signatureDecoder.GetDecodedSignature(methodDef);

            var import = methodDef.GetImport();
            var moduleReference = base._mdReader.GetModuleReference(import.Module);
            var dllLocation = base._mdReader.GetString(moduleReference.Name);
            var dllImportArguments = import.Attributes.ToString();

            return new ImportedMethodInfo
            {
                SignatureInfo = signatureInfo,
                DllLocation = dllLocation,
                DllImportArguments = dllImportArguments
            };
        }

    }
}
