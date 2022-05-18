using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Decoders;
using Consyzer.AnalyzerEngine.Analyzers.SyntaxModels;
using Consyzer.AnalyzerEngine.CommonModels;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    /// <summary>
    /// [Sealed] Provides tools for analyzing CLI metadata.
    /// </summary>
    public sealed class MetadataAnalyzer
    {
        /// <summary>
        /// Gets the <b>BinaryFileInfo</b> instance that contains detailed information about the binary file being analyzed.
        /// </summary>
        public BinaryFileInfo BinaryInfo { get; }
        /// <summary>
        /// Gets a <b>MetadataReader</b> instance representing the current PE file being processed. 
        /// </summary>
        public MetadataReader MdReader { get; }

        /// <summary>
        /// Initializes a new instance of <b>MetadataAnalyzer</b>.
        /// </summary>
        /// <param name="binary"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="MetadataFileNotSupportedException"></exception>
        /// <exception cref="AssemblyFileNotSupportedException"></exception>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
            if (!this.BinaryInfo.IsMetadataAssembly)
            {
                throw new AssemblyFileNotSupportedException($"{binary.BaseFileInfo.FullName} is contains metadata, but is not an assembly.");
            }

            this.MdReader = new PEReader(new FileStream(this.BinaryInfo.BaseFileInfo.FullName, FileMode.Open, FileAccess.Read)).GetMetadataReader();
        }

        /// <summary>
        /// Initializes a new instance of <b>MetadataAnalyzer</b>.
        /// </summary>
        /// <param name="pathToBinary"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="MetadataFileNotSupportedException"></exception>
        /// <exception cref="AssemblyFileNotSupportedException"></exception>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
            if (!this.BinaryInfo.IsMetadataAssembly)
            {
                throw new AssemblyFileNotSupportedException($"{nameof(pathToBinary)} is contains metadata, but is not an assembly.");
            }

            this.MdReader = new PEReader(new FileStream(this.BinaryInfo.BaseFileInfo.FullName, FileMode.Open, FileAccess.Read)).GetMetadataReader();
        }

        /// <summary>
        /// Returns a list of methods definitions handles imported from other assemblies.
        /// </summary>
        /// <returns><b>IEnumerable&lt;MethodDefinitionHandle&gt;</b> collection.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public IEnumerable<MethodDefinitionHandle> GetImportedMethodsDefinitionsHandles()
        {
            var importedMethods = new List<MethodDefinitionHandle>();
            foreach(var methodHandle in this.GetMethodsDefinitionsHandles())
            {
                var method = this.MdReader.GetMethodDefinition(methodHandle);

                var import = method.GetImport();
                if (!import.Name.IsNil || !import.Module.IsNil)
                {
                    importedMethods.Add(methodHandle);
                }
            }

            return importedMethods;
        }

        /// <summary>
        /// Returns a list of methods definitions imported from other assemblies.
        /// </summary>
        /// <returns><b>IEnumerable&lt;MethodDefinition&gt;</b> collection.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

        /// <summary>
        /// Returns the <b>ImportedMethodInfo</b> collection that contains detailed information about all methods in the assembly imported by their other assembly.
        /// </summary>
        /// <returns><b>IEnumerable&lt;ImportedMethodInfo&gt;</b> collection.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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

        /// <summary>
        /// Returns a collection of all types definitions handles in an assembly.
        /// </summary>
        /// <returns><b>IEnumerable&lt;TypeDefinitionHandle&gt;</b> collection.</returns>
        public IEnumerable<TypeDefinitionHandle> GetTypesDefinitionsHandles()
        {
            var handles = new List<TypeDefinitionHandle>();
            foreach (var typeHandle in this.MdReader.TypeDefinitions)
            {
                handles.Add(typeHandle);
            }

            return handles;
        }

        /// <summary>
        /// Returns a collection of all types definitions in an assembly.
        /// </summary>
        /// <returns><b>IEnumerable&lt;TypeDefinition&gt;</b> collection.</returns>
        public IEnumerable<TypeDefinition> GetTypesDefinitions()
        {
            var defs = new List<TypeDefinition>();
            foreach (var typeDef in this.MdReader.TypeDefinitions.Select(h => this.MdReader.GetTypeDefinition(h)))
            {
                defs.Add(typeDef);
            }

            return defs;
        }

        /// <summary>
        /// Returns a collection of all methods definitions handle in an assembly.
        /// </summary>
        /// <returns><b>IEnumerable&lt;MethodDefinitionHandle&gt;</b> collection.</returns>
        public IEnumerable<MethodDefinitionHandle> GetMethodsDefinitionsHandles()
        {
            var handles = new List<MethodDefinitionHandle>();
            foreach (var typeDef in this.GetTypesDefinitions())
            {
                foreach (var methodHandle in typeDef.GetMethods())
                {
                    handles.Add(methodHandle);
                }
            }

            return handles;
        }

        /// <summary>
        /// Returns a collection of all methods definitions in an assembly.
        /// </summary>
        /// <returns><b>IEnumerable&lt;MethodDefinition&gt;</b> collection.</returns>
        public IEnumerable<MethodDefinition> GetMethodsDefinitions()
        {
            var defs = new List<MethodDefinition>();
            foreach (var typeDef in this.GetTypesDefinitions())
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
