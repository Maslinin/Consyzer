using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal sealed class MetadataImportedMethodExtractor : IEcmaImportedMethodExtractor
{
    private readonly IEcmaDefinitionExtractor _definitionExtractor;

    public FileInfo MetadataAssembly { get; }

    public MetadataImportedMethodExtractor(FileInfo fileInfo)
    {
        this.MetadataAssembly = fileInfo;
        this._definitionExtractor = new MetadataDefinitionExtractor(this.MetadataAssembly);
    }

    public IEnumerable<ImportedMethodInfo> GetImportedMethodInfos()
    {
        var importedMethodDefs = this._definitionExtractor.GetImportedMethodDefinitions();
        return importedMethodDefs.Select(GetImportedMethodInfo);
    }

    private ImportedMethodInfo GetImportedMethodInfo(MethodDefinition methodDef)
    {
        var importedMethod = methodDef.GetImport();

        using var fileStream = this.MetadataAssembly.OpenRead();
        using var peReader = new PEReader(fileStream);
        var mdReader = peReader.GetMetadataReader();

        return new ImportedMethodInfo
        {
            Signature = GetSignature(mdReader, methodDef),
            DllLocation = GetDllLocation(mdReader, importedMethod),
            DllImportArgs = importedMethod.Attributes.ToString()
        };
    }

    private static SignatureInfo GetSignature(MetadataReader mdReader, MethodDefinition methodDef)
    {
        var signatureExtractor = new MetadataSignatureExtractor(mdReader);
        return signatureExtractor.GetSignature(methodDef);
    }

    private static string GetDllLocation(MetadataReader mdReader, MethodImport importedMethod)
    {
        var moduleReference = mdReader.GetModuleReference(importedMethod.Module);
        return mdReader.GetString(moduleReference.Name);
    }
}