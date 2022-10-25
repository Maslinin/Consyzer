using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Consyzer.AnalyzerEngine.Decoders.Models;

[assembly: InternalsVisibleTo("Consyzer.AnalyzerEngine.Tests")]

namespace Consyzer.AnalyzerEngine.Decoders
{
    internal sealed class MethodInfoExtractor
    {
        private readonly MetadataReader _mdReader;
        
        public MethodInfoExtractor(MetadataReader mdReader)
        {
            this._mdReader = mdReader;
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

        public AccessibilityModifiers GetMethodAccessibilityModifier(MethodDefinition methodDef)
        {
            string methodAttributes = this.GetMethodAttributes(methodDef);
            var separatedMethodAttributes = methodAttributes.Split(',').Select(x => x.Trim());

            string[] accessibilityModifiers = Enum.GetNames(typeof(CILAccessibilityModifiers));
            var modifierAsString = separatedMethodAttributes.Intersect(accessibilityModifiers).First();

            var parsedModifier = Enum.Parse(typeof(CILAccessibilityModifiers), modifierAsString);
            return (AccessibilityModifiers)(int)parsedModifier;
        }

        public bool IsStaticMethod(MethodDefinition methodDef)
        {
            var methodAttributes = this.GetMethodAttributes(methodDef);
            var separatedMethodAttributes = methodAttributes.Split(',').Select(x => x.Trim());

            return separatedMethodAttributes.Any(s => s == "Static");
        }

        public string GetMethodAttributes(MethodDefinition methodDef)
        {
            return methodDef.Attributes.ToString();
        }

        public string GetMethodImplAttributes(MethodDefinition methodDef)
        {
            return methodDef.ImplAttributes.ToString();
        }

        public ISignatureParameterType GetMethodReturnType(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureDecoderTypeProvider(this._mdReader, methodDef);
            var signature = methodDef.DecodeSignature(signatureProvider, new object());

            return signature.ReturnType;
        }

        public IEnumerable<ISignatureParameterType> GetMethodArguments(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureDecoderTypeProvider(this._mdReader, methodDef);
            var signature = methodDef.DecodeSignature(signatureProvider, new object());

            return signature.ParameterTypes;
        }
    }
}
