using System.Reflection;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Consyzer.Core.Models;
using Consyzer.Core.Extractors.Providers;

namespace Consyzer.Core.Extractors;

internal sealed class MethodSignatureExtractor(
    MetadataReader mdReader
) : IExtractor<MethodDefinition, MethodSignature>
{
    public MethodSignature Extract(MethodDefinition methodDef)
    {
        return new MethodSignature
        {
            ReturnType = GetReturnType(methodDef),
            IsStatic = IsStatic(methodDef),
            Namespace = GetNamespace(methodDef),
            Class = GetClassName(methodDef),
            Method = GetMethodName(methodDef),
            MethodArguments = GetArguments(methodDef)
        };
    }

    private static string GetReturnType(MethodDefinition methodDef)
    {
        var signature = DecodeSignature(methodDef);
        return signature.ReturnType;
    }

    public static bool IsStatic(MethodDefinition methodDef)
    {
        return methodDef.Attributes.HasFlag(MethodAttributes.Static);
    }

    private string GetNamespace(MethodDefinition methodDef)
    {
        var typeDef = GetDeclaringTypeDefinition(methodDef);
        return mdReader.GetString(typeDef.Namespace);
    }

    private string GetClassName(MethodDefinition methodDef)
    {
        var typeDef = GetDeclaringTypeDefinition(methodDef);
        return mdReader.GetString(typeDef.Name);
    }

    private string GetMethodName(MethodDefinition methodDef)
    {
        return mdReader.GetString(methodDef.Name);
    }

    private static ImmutableArray<string> GetArguments(MethodDefinition methodDef)
    {
        var signature = DecodeSignature(methodDef);
        return signature.ParameterTypes;
    }

    private TypeDefinition GetDeclaringTypeDefinition(MethodDefinition methodDef)
    {
        return mdReader.GetTypeDefinition(methodDef.GetDeclaringType());
    }

    private static MethodSignature<string> DecodeSignature(MethodDefinition methodDef)
    {
        var signatureProvider = new StringSignatureTypeProvider();
        return methodDef.DecodeSignature(signatureProvider, new object());
    }
}