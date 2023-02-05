using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Analyzers.Models;

namespace Consyzer.Analyzers
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
        /// <summary>
        /// Returns a collection <b>ImportedMethodInfo</b> containing detailed information about all methods in the assembly.
        /// </summary>
        IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo();
        /// <summary>
        /// Returns a list of methods definitions imported from other assemblies.
        /// </summary>
        IEnumerable<MethodDefinition> GetImportedMethodsDefinitions();
    }
}
