using System.Reflection.Metadata;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal sealed class MetadataSignatureExtractor : IEcmaSignatureExtractor
{
    private readonly MetadataReader _mdReader;

    public MetadataSignatureExtractor(MetadataReader mdReader)
    {
        this._mdReader = mdReader;
    }

    public SignatureInfo GetSignature(MethodDefinition methodDef)
    {
        return new SignatureInfo
        {
            Namespace = GetNamespace(methodDef),
            Class = GetClassName(methodDef),
            Method = GetMethodName(methodDef),
            MethodArguments = GetArguments(methodDef)
        };
    }

    private string GetNamespace(MethodDefinition methodDef)
    {
        var typeDef = this._mdReader.GetTypeDefinition(methodDef.GetDeclaringType());
        return this._mdReader.GetString(typeDef.Namespace);
    }

    private string GetClassName(MethodDefinition methodDef)
    {
        var typeDef = this._mdReader.GetTypeDefinition(methodDef.GetDeclaringType());
        return this._mdReader.GetString(typeDef.Name);
    }

    private string GetMethodName(MethodDefinition methodDef)
    {
        return this._mdReader.GetString(methodDef.Name);
    }

    private static IEnumerable<string> GetArguments(MethodDefinition methodDef)
    {
        var signature = DecodeSignature(methodDef);
        return signature.ParameterTypes;
    }

    private static MethodSignature<string> DecodeSignature(MethodDefinition methodDef)
    {
        var signatureProvider = new SignatureTypeProviderForString();
        return methodDef.DecodeSignature(signatureProvider, new object());
    }
}