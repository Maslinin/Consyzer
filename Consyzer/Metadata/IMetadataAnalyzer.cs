using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Metadata.Models;

namespace Consyzer.Metadata
{
    internal interface IMetadataAnalyzer
    {
        FileInfo FileInfo { get; }
        IEnumerable<MethodDefinition> GetMethodDefinitions();
        IEnumerable<ImportedMethodInfo> GetImportedMethodsInfo();
        IEnumerable<MethodDefinition> GetImportedMethodDefinitions();
    }
}
