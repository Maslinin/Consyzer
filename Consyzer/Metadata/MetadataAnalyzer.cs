using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Metadata.Models;

namespace Consyzer.Metadata
{
    internal sealed class MetadataAnalyzer
    {
        private readonly MetadataReader _mdReader;

        public FileInfo FileInfo { get; }

        public MetadataAnalyzer(FileInfo fileInfo)
        {
            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            this.FileInfo = fileInfo;

            var peReader = new PEReader(fileStream);
            this._mdReader = peReader.GetMetadataReader();
        }

        public IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo()
        {
            var importedMethodDefs = this.GetImportedMethodDefinitions();
            return importedMethodDefs.Select(m => this.GetImportedMethodInfo(m));
        }

        public IEnumerable<MethodDefinition> GetImportedMethodDefinitions()
        {
            var methodDefs = this.GetMethodDefinitions();
            return methodDefs.Where(m => this.IsImportedMethod(m));
        }

        public IEnumerable<MethodDefinition> GetMethodDefinitions()
        {
            var methodDefHandles = this._mdReader.MethodDefinitions;
            return methodDefHandles.Select(h => this._mdReader.GetMethodDefinition(h));
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
            var import = methodDef.GetImport();
            var signatureDecoder = new MethodInfoExtractor(this._mdReader);

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
