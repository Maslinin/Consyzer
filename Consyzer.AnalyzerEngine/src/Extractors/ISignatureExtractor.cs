using Consyzer.AnalyzerEngine.Decoders.Models;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Consyzer.AnalyzerEngine.Decoders
{
    /// <summary>
    /// Provides methods for decoding signatures represented by the ECMA-355 standard.
    /// </summary>
    public interface ISignatureExtractor
    {
        /// <summary>
        /// Decodes the method signature and returns a <b>SignatureInfo</b> instance containing the decoded signature information.
        /// </summary>
        /// <param name="methodDef"></param>
        /// <returns>A SignatureInfo instance containing information about method signature.</returns>
        SignatureInfo GetDecodedSignature(MethodDefinition methodDef);
        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="methodDefs"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing information about decoded signatures.</returns>
        IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodsDefs);
        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeDef"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing information about decoded signatures.</returns>
        IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinition typeDef);
        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeDefs"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing information about decoded signatures.</returns>
        IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinition> typeDefs);
    }
}
