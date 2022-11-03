using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.AnalyzerEngine.Exceptions;
using Consyzer.AnalyzerEngine.Signature.Models;

namespace Consyzer.AnalyzerEngine.Signature
{
    /// <summary>
    /// Contains tools for decoding a method signature.
    /// </summary>
    public class SignatureExtractor : ISignatureExtractor
    {
        private readonly MetadataReader _mdReader;

        public SignatureExtractor(FileInfo fileInfo)
        {
            ExceptionThrower.ThrowExceptionIfFileDoesNotExist(fileInfo);
            ExceptionThrower.ThrowExceptionIfFileIsNotMetadataAssembly(fileInfo);

            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            var peReader = new PEReader(fileStream);

            this._mdReader = peReader.GetMetadataReader();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal SignatureExtractor(MetadataReader mdReader)
        {
            this._mdReader = mdReader;
        }

        #region Get Decoded Signatures From Types Defs

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinition> typeDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var typeDef in typeDefs)
            {
                decodedSignatures.AddRange(this.GetDecodedSignatures(typeDef));
            }

            return decodedSignatures;
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinition typeDef)
        {
            var methodsDefsHandles = typeDef.GetMethods();
            var methodsDefs = methodsDefsHandles.Select(x => this._mdReader.GetMethodDefinition(x));

            return this.GetDecodedSignatures(methodsDefs);
        }

        #endregion

        #region Get Decoded Signatures From Methods Defs

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodsDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var methodDef in methodsDefs)
            {
                decodedSignatures.Add(this.GetDecodedSignature(methodDef));
            }

            return decodedSignatures;
        }

        public SignatureInfo GetDecodedSignature(MethodDefinition methodDef)
        {
            var extractor = new MethodInfoExtractor(this._mdReader);

            return new SignatureInfo
            {
                Namespace = extractor.GetNamespace(methodDef),
                ClassName = extractor.GetClassName(methodDef),
                MethodName = extractor.GetMethodName(methodDef),
                Accessibility = extractor.GetMethodAccessibilityModifier(methodDef),
                IsStatic = extractor.IsStaticMethod(methodDef),
                ReturnType = extractor.GetMethodReturnType(methodDef),
                MethodArguments = extractor.GetMethodArguments(methodDef),
                MethodAttributes = extractor.GetMethodAttributes(methodDef),
                MethodImplAttributes = extractor.GetMethodImplAttributes(methodDef)
            };
        }

        #endregion

    }
}
