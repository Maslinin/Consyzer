﻿using System.Collections.Generic;
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

    }
}