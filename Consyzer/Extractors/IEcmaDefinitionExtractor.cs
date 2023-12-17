using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Consyzer.Extractors;

internal interface IEcmaDefinitionExtractor
{
    IEnumerable<MethodDefinition> GetMethodDefinitions();
    IEnumerable<MethodDefinition> GetImportedMethodDefinitions();
}