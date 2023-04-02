using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Collections.Generic;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors
{
    internal class ImportedMethodExtractor : MetadataExtractor
    {
        public FileInfo FileInfo { get; }

        public ImportedMethodExtractor(FileInfo fileInfo) : base(fileInfo)
        {
            FileInfo = fileInfo;
        }

        public IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo()
        {
            var importedMethodDefs = GetImportedMethodDefinitions();
            return importedMethodDefs.Select(m => GetImportedSignatureInfo(m));
        }

        public IEnumerable<MethodDefinition> GetImportedMethodDefinitions()
        {
            var methodDefs = this.GetMethodDefinitions();
            return methodDefs.Where(m => this.IsImportedMethod(m));
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private bool IsImportedMethod(MethodDefinition methodDef)
        {
            var import = methodDef.GetImport();
            return !import.Name.IsNil && !import.Module.IsNil;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private ImportedMethodInfo GetImportedSignatureInfo(MethodDefinition methodDef)
        {
            var import = methodDef.GetImport();
            var signatureDecoder = new SignatureInfoExtractor(this._mdReader);

            var signatureInfo = signatureDecoder.GetSignatureInfo(methodDef);
            var moduleReference = this._mdReader.GetModuleReference(import.Module);
            var dllLocation = this._mdReader.GetString(moduleReference.Name);
            var dllImportArgs = import.Attributes.ToString();

            return new ImportedMethodInfo
            {
                Signature = signatureInfo,
                DllLocation = dllLocation,
                DllImportArgs = dllImportArgs
            };
        }
    }
}
