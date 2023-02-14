using System;
using System.Linq;
using System.Collections.Generic;
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
            string methodAttributes = this.GetMethodAttributes(methodDef);
            var separatedMethodAttributes = this.GetSeparatedMethodAttributes(methodAttributes);

            string[] accessibilityModifiers = Enum.GetNames(typeof(MsilAccessModifier));
            var modifierAsString = separatedMethodAttributes.Intersect(accessibilityModifiers).First();

            var parsedModifier = Enum.Parse(typeof(MsilAccessModifier), modifierAsString);
            return (AccessModifier)(int)parsedModifier;
        }

        public bool IsStaticMethod(MethodDefinition methodDef)
        {
            var methodAttributes = this.GetMethodAttributes(methodDef);
            var separatedMethodAttributes = this.GetSeparatedMethodAttributes(methodAttributes);

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

        public SignatureParameter GetMethodReturnType(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureTypeProvider(this._mdReader, methodDef);
            var signature = methodDef.DecodeSignature(signatureProvider, new object());

            return signature.ReturnType;
        }

        public IEnumerable<SignatureParameter> GetMethodArguments(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureTypeProvider(this._mdReader, methodDef);
            var signature = methodDef.DecodeSignature(signatureProvider, new object());

            return signature.ParameterTypes;
        }

        private IEnumerable<string> GetSeparatedMethodAttributes(string methodAttributes)
        {
            return methodAttributes.Split(',').Select(a => a.Trim());
        }
    }
}
