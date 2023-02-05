using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Exceptions;
using Consyzer.Analyzers.Models;
using Consyzer.Signature;

namespace Consyzer.Analyzers
{
    /// <summary>
    /// Provides tools for analyzing CLI metadata.
    /// </summary>
    internal class MetadataAnalyzer : IMetadataAnalyzer
    {
        protected readonly MetadataReader _mdReader;

        public FileInfo FileInfo { get; }

        public MetadataAnalyzer(FileInfo fileInfo)
        {
            ExceptionThrower.ThrowExceptionIfFileDoesNotExist(fileInfo);
            ExceptionThrower.ThrowExceptionIfFileIsNotMetadataAssembly(fileInfo);

            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            var peReader = new PEReader(fileStream);

            this._mdReader = peReader.GetMetadataReader();
            this.FileInfo = fileInfo;
        }

        public IEnumerable<MethodDefinition> GetMethodsDefinitions()
        {
            var methodsDefs = new List<MethodDefinition>();

            foreach (var typeDef in GetTypesDefinitions())
            {
                var methodDefsHandles = typeDef.GetMethods();
                var defs = methodDefsHandles.Select(h => this._mdReader.GetMethodDefinition(h));

                methodsDefs.AddRange(defs);
            }

            return methodsDefs;
        }

        public IEnumerable<TypeDefinition> GetTypesDefinitions()
        {
            var typeDefsHandles = this._mdReader.TypeDefinitions;
            var typesDefs = typeDefsHandles.Select(h => this._mdReader.GetTypeDefinition(h));

            return typesDefs;
        }

        public IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo()
        {
            return this.GetImportedMethodsDefinitions().Select(x => this.GetImportedMethodInfo(x));
        }

        public IEnumerable<MethodDefinition> GetImportedMethodsDefinitions()
        {
            return this.GetMethodsDefinitions().Where(x => this.IsImportedMethod(x));
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
            var signatureDecoder = new SignatureExtractor(this._mdReader);
            var signatureInfo = signatureDecoder.GetDecodedSignature(methodDef);

            var import = methodDef.GetImport();
            var moduleReference = this._mdReader.GetModuleReference(import.Module);
            var dllLocation = this._mdReader.GetString(moduleReference.Name);
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
