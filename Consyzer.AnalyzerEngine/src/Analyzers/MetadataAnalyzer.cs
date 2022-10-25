using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Exceptions;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    /// <summary>
    /// Provides tools for analyzing CLI metadata.
    /// </summary>
    public class MetadataAnalyzer : IMetadataAnalyzer
    {
        public FileInfo FileInfo { get; }

        protected readonly MetadataReader _mdReader;

        public MetadataAnalyzer(FileInfo fileInfo)
        {
            ExceptionThrower.ThrowExceptionIfFileDoesNotExists(fileInfo);
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

    }
}
