using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.Extractors;

internal sealed class MetadataDefinitionExtractor : IEcmaDefinitionExtractor
{
    private readonly MetadataReader _mdReader;

    public MetadataDefinitionExtractor(FileInfo fileInfo)
    {
        var fileStream = fileInfo.OpenRead();
        var peReader = new PEReader(fileStream);
        this._mdReader = peReader.GetMetadataReader();
    }

    public IEnumerable<MethodDefinition> GetMethodDefinitions()
    {
        var methodDefHandles = this._mdReader.MethodDefinitions;
        return methodDefHandles.Select(this._mdReader.GetMethodDefinition);
    }

    public IEnumerable<MethodDefinition> GetImportedMethodDefinitions()
    {
        var methodDefs = this.GetMethodDefinitions();
        return methodDefs.Where(IsImported);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static bool IsImported(MethodDefinition methodDef)
    {
        var importedMethod = methodDef.GetImport();
        return !(importedMethod.Name.IsNil && importedMethod.Module.IsNil);
    }
}