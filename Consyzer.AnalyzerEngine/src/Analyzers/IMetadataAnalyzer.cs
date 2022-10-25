using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Consyzer.AnalyzerEngine.Analyzers
{
    public interface IMetadataAnalyzer
    {
        /// <summary>
        /// Returns a FileInfo instance containing information about the file being analyzed.
        /// </summary>
        FileInfo FileInfo { get; }
        /// <summary>
        /// Returns a collection of all types definitions in an assembly.
        /// </summary>
        IEnumerable<TypeDefinition> GetTypesDefinitions();
        /// <summary>
        /// Returns a collection of all methods definitions in an assembly.
        /// </summary>
        IEnumerable<MethodDefinition> GetMethodsDefinitions();
    }
}
