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
            Namespace = this.GetNamespace(methodDef),
            Class = this.GetClassName(methodDef),
            Method = this.GetMethodName(methodDef),
            MethodArguments = this.GetArguments(methodDef)
        };
    }

    public string GetNamespace(MethodDefinition methodDef)
    {
        var typeDef = this._mdReader.GetTypeDefinition(methodDef.GetDeclaringType());
        return this._mdReader.GetString(typeDef.Namespace);
    }

    public string GetClassName(MethodDefinition methodDef)
    {
        var typeDef = this._mdReader.GetTypeDefinition(methodDef.GetDeclaringType());
        return this._mdReader.GetString(typeDef.Name);
    }

    public string GetMethodName(MethodDefinition methodDef)
    {
        return this._mdReader.GetString(methodDef.Name);
    }

    public IEnumerable<string> GetArguments(MethodDefinition methodDef)
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