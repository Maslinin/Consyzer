using System.Reflection.Metadata;

namespace Consyzer.Extractors;

internal interface IEcmaDefinitionExtractor
{
    FileInfo MetadataAssembly { get; }
    IEnumerable<MethodDefinition> GetMethodDefinitions();
    IEnumerable<MethodDefinition> GetImportedMethodDefinitions();
}