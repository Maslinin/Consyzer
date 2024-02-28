using System.Reflection.Metadata;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal interface IEcmaSignatureExtractor
{
    SignatureInfo GetSignature(MethodDefinition methodDef);
}