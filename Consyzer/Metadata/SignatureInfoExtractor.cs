using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Collections.Generic;
using Consyzer.Metadata.Models;

namespace Consyzer.Metadata
{
    internal sealed class SignatureInfoExtractor
    {
        private readonly MetadataReader _mdReader;
        
        public SignatureInfoExtractor(MetadataReader mdReader)
        {
            this._mdReader = mdReader;
        }

        public SignatureInfo GetSignatureInfo(MethodDefinition methodDef)
        {
            return new SignatureInfo
            {
                Namespace = this.GetNamespace(methodDef),
                ClassName = this.GetClassName(methodDef),
                MethodName = this.GetMethodName(methodDef),
                Accessibility = this.GetMethodAccessibilityModifier(methodDef),
                IsStatic = this.IsStaticMethod(methodDef),
                ReturnType = this.GetMethodReturnType(methodDef),
                MethodArguments = this.GetMethodArguments(methodDef)
            };
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
            var methodAttributes = methodDef.Attributes & MethodAttributes.MemberAccessMask;
            return methodAttributes switch
            {
                MethodAttributes.Private => AccessModifier.Private,
                MethodAttributes.Public => AccessModifier.Public,
                MethodAttributes.Assembly => AccessModifier.Internal,
                MethodAttributes.Family => AccessModifier.Protected,
                MethodAttributes.FamORAssem => AccessModifier.ProtectedInternal,
                MethodAttributes.FamANDAssem => AccessModifier.ProtectedPrivate,
                _ => throw new InvalidOperationException("Invalid method accessibility modifier."),
            };
        }

        public bool IsStaticMethod(MethodDefinition methodDef)
        {
            return (methodDef.Attributes & MethodAttributes.Static) != 0;
        }

        public SignatureParameter GetMethodReturnType(MethodDefinition methodDef)
        {
            var signature = this.DecodeSignature(methodDef);
            return signature.ReturnType;
        }

        public IEnumerable<SignatureParameter> GetMethodArguments(MethodDefinition methodDef)
        {
            var signature = this.DecodeSignature(methodDef);
            return signature.ParameterTypes;
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private MethodSignature<SignatureParameter> DecodeSignature(MethodDefinition methodDef)
        {
            var signatureProvider = new SignatureParameterTypeProvider(this._mdReader, methodDef);
            return methodDef.DecodeSignature(signatureProvider, new object());
        }

    }
}
