using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Decoder;
using Consyzer.AnalyzerEngine.Analyzer.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Analyzer
{
    public sealed class MetadataAnalyzer
    {
        public BinaryFileInfo BinaryInfo { get; }
        public MetadataReader MdReader { get; }

        public MetadataAnalyzer(BinaryFileInfo binary)
        {
            if (binary is null)
            {
                throw new System.ArgumentException($"{nameof(binary)} is null.");
            }

            this.BinaryInfo = binary;

            if (!this.BinaryInfo.HasMetadata)
            {
                throw new MetadataFileNotSupportedException($"{binary.BaseFileInfo.FullName} is does not contain metadata.");
            }
            if (!this.BinaryInfo.IsAssembly)
            {
                throw new AssemblyFileNotSupportedException($"{binary.BaseFileInfo.FullName} is contains metadata, but is not an assembly.");
            }

            this.MdReader = new PEReader(new FileStream(this.BinaryInfo.BaseFileInfo.FullName, FileMode.Open, FileAccess.Read)).GetMetadataReader();
        }

        public MetadataAnalyzer(string pathToBinary)
        {
            if (!File.Exists(pathToBinary))
            {
                throw new FileNotFoundException($"file {nameof(pathToBinary)} is not found.");
            }

            this.BinaryInfo = new BinaryFileInfo(pathToBinary);

            if (!this.BinaryInfo.HasMetadata)
            {
                throw new MetadataFileNotSupportedException($"{nameof(pathToBinary)} is does not contain metadata.");
            }
            if (!this.BinaryInfo.IsAssembly)
            {
                throw new AssemblyFileNotSupportedException($"{nameof(pathToBinary)} is contains metadata, but is not an assembly.");
            }

            this.MdReader = new PEReader(new FileStream(this.BinaryInfo.BaseFileInfo.FullName, FileMode.Open, FileAccess.Read)).GetMetadataReader();
        }

        public IEnumerable<MethodDefinition> GetImportedMethodsDefinitions()
        {
            var importedMethods = new List<MethodDefinition>();
            foreach (var method in this.GetMethodsDefinitions())
            {
                var import = method.GetImport();
                if (!import.Name.IsNil || !import.Module.IsNil)
                {
                    importedMethods.Add(method);
                }
            }

            return importedMethods;
        }

        public IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo()
        {
            var dllImports = new List<ImportedMethodInfo>();

            foreach (var methodDef in this.GetImportedMethodsDefinitions())
            {
                var import = methodDef.GetImport();
                var signature = new SignatureDecoder(this.MdReader).GetDecodedSignature(methodDef);

                dllImports.Add(new ImportedMethodInfo(signature, this.MdReader.GetString(this.MdReader.GetModuleReference(import.Module).Name), import.Attributes.ToString()));
            }

            return dllImports;
        }

        public IEnumerable<TypeDefinitionHandle> GetTypesDefinitionsHandles()
        {
            var handles = new List<TypeDefinitionHandle>();
            foreach (var typeHandle in this.MdReader.TypeDefinitions)
            {
                handles.Add(typeHandle);
            }

            return handles;
        }

        public IEnumerable<TypeDefinition> GetTypesDefinitions()
        {
            var defs = new List<TypeDefinition>();
            foreach (var typeDef in this.MdReader.TypeDefinitions.Select(h => this.MdReader.GetTypeDefinition(h)))
            {
                defs.Add(typeDef);
            }

            return defs;
        }

        public IEnumerable<MethodDefinitionHandle> GetMethodsDefinitionsHandles()
        {
            var handles = new List<MethodDefinitionHandle>();
            foreach (var typeDef in this.MdReader.TypeDefinitions.Select(h => this.MdReader.GetTypeDefinition(h)))
            {
                foreach (var methodHandle in typeDef.GetMethods())
                {
                    handles.Add(methodHandle);
                }
            }

            return handles;
        }

        public IEnumerable<MethodDefinition> GetMethodsDefinitions()
        {
            var defs = new List<MethodDefinition>();
            foreach (var typeDef in this.MdReader.TypeDefinitions.Select(h => this.MdReader.GetTypeDefinition(h)))
            {
                foreach (var methodDef in typeDef.GetMethods().Select(h => this.MdReader.GetMethodDefinition(h)))
                {
                    defs.Add(methodDef);
                }
            }

            return defs;
        }

    }
}
