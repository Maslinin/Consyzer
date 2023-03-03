using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using Consyzer.Metadata.Models;

namespace Consyzer.Metadata
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

        public AccessModifier GetMethodAccessibilityModifier(MethodDefinition methodDef)
        {
            var separatedMethodAttributes = this.GetSeparatedMethodAttributes(methodDef);

            string[] accessibilityModifiers = Enum.GetNames(typeof(MsilAccessModifier));
            var modifierAsString = separatedMethodAttributes.Intersect(accessibilityModifiers).First();

            var parsedModifier = Enum.Parse(typeof(MsilAccessModifier), modifierAsString);
            return (AccessModifier)(int)parsedModifier;
        }

        public bool IsStaticMethod(MethodDefinition methodDef)
        {
            var separatedMethodAttributes = this.GetSeparatedMethodAttributes(methodDef);
            return separatedMethodAttributes.Any(s => s == nameof(MethodAttributes.Static));
        }

        public SignatureParameter GetMethodReturnType(MethodDefinition methodDef)
        {
            var signature = DecodeSignature(methodDef);
            return signature.ReturnType;
        }

        public IEnumerable<SignatureParameter> GetMethodArguments(MethodDefinition methodDef)
        {
            var signature = DecodeSignature(methodDef);
            return signature.ParameterTypes;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private IEnumerable<string> GetSeparatedMethodAttributes(MethodDefinition methodDef)
        {
            string methodAttributes = methodDef.Attributes.ToString();
            return methodAttributes.Split(',').Select(a => a.Trim());
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private MethodSignature<SignatureParameter> DecodeSignature(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureParameterTypeProvider(this._mdReader, methodDef);
            return methodDef.DecodeSignature(signatureProvider, new object());
        }

    }
}
