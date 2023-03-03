using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.Metadata.Models;

namespace Consyzer.Metadata
{
    internal class SignatureExtractor
    {
        private readonly MetadataReader _mdReader;

        internal SignatureExtractor(MetadataReader mdReader)
        {
            this._mdReader = mdReader;
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodsDefs)
        {
            return methodsDefs.Select(m => this.GetDecodedSignature(m));
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
                MethodArguments = extractor.GetMethodArguments(methodDef)
            };
        }

    }
}
