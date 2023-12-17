using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal sealed class MetadataImportedMethodExtractor : IEcmaImportedMethodExtractor
{
    private readonly FileInfo _fileInfo;
    private readonly IEcmaDefinitionExtractor _definitionExtractor;

    public MetadataImportedMethodExtractor(FileInfo fileInfo)
    {
        this._fileInfo = fileInfo;
        this._definitionExtractor = new MetadataDefinitionExtractor(fileInfo);
    }

    public IEnumerable<ImportedMethodInfo> GetImportedMethodInfos()
    {
        var importedMethodDefs = this._definitionExtractor.GetImportedMethodDefinitions();
        return importedMethodDefs.Select(GetImportedMethodInfo);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private ImportedMethodInfo GetImportedMethodInfo(MethodDefinition methodDef)
    {
        var importedMethod = methodDef.GetImport();

        using var fileStream = this._fileInfo.OpenRead();
        using var peReader = new PEReader(fileStream);
        var mdReader = peReader.GetMetadataReader();

        return new ImportedMethodInfo
        {
            Signature = GetSignature(mdReader, methodDef),
            DllLocation = GetDllLocation(mdReader, importedMethod),
            DllImportArgs = importedMethod.Attributes.ToString()
        };
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static SignatureInfo GetSignature(MetadataReader mdReader, MethodDefinition methodDef)
    {
        var signatureExtractor = new MetadataSignatureExtractor(mdReader);
        return signatureExtractor.GetSignature(methodDef);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private static string GetDllLocation(MetadataReader mdReader, MethodImport importedMethod)
    {
        var moduleReference = mdReader.GetModuleReference(importedMethod.Module);
        return mdReader.GetString(moduleReference.Name);
    }
}