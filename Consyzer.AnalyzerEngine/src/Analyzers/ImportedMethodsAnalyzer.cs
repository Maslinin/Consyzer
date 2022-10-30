using System.IO;
using System.Linq;
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
            return this.GetImportedMethodsDefinitions().Select(x => this.GetImportedMethodInfo(x));
        }

        /// <summary>
        /// Returns a list of methods definitions imported from other assemblies.
        /// </summary>
        public IEnumerable<MethodDefinition> GetImportedMethodsDefinitions()
        {
            return base.GetMethodsDefinitions().Where(x => this.IsImportedMethod(x));
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
            var signatureDecoder = new SignatureExtractor(base._mdReader);
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
