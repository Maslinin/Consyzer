using System.Reflection.Metadata;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal interface IEcmaSignatureExtractor
{
    SignatureInfo GetSignature(MethodDefinition methodDef);
    string GetNamespace(MethodDefinition methodDef);
    string GetClassName(MethodDefinition methodDef);
    string GetMethodName(MethodDefinition methodDef);
    IEnumerable<string> GetArguments(MethodDefinition methodDef);
}