using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.AnalyzerEngine.Decoder.Provider;
using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Decoder
{
    public sealed class SignatureDecoder
    {
        private readonly MetadataReader _reader;

        public SignatureDecoder(MetadataReader mdReader)
        {
            this._reader = mdReader;
        }

        public SignatureInfo GetDecodedSignature(MethodDefinition methodDef)
        {
            var typeDef = this._reader.GetTypeDefinition(methodDef.GetDeclaringType());
            var signature = methodDef.DecodeSignature(new SignatureDecoderTypeProvider(this._reader, methodDef), new object());

            //Namespace.Class.MethodName:
            string @namespace = this._reader.GetString(typeDef.Namespace), @class = this._reader.GetString(typeDef.Name), @methodName = this._reader.GetString(methodDef.Name);

            string fullMethodAttributes = methodDef.Attributes.ToString();

            //Getting Method Access Modifier:
            var methodAttributes = fullMethodAttributes.Split(',').Select(s => s.Trim()).ToList();
            string methodAccessibility = string.Empty;
            foreach (AccessibilityModifiersNotTranslated modifier in Enum.GetValues(typeof(AccessibilityModifiersNotTranslated)))
            {
                if (methodAttributes.Any(s => s.ToLower() == modifier.ToString().ToLower()))
                {
                    methodAccessibility = ((AccessibilityModifiers)Enum.GetValues(typeof(AccessibilityModifiers)).GetValue((int)modifier)).ToString();
                }
            }

            //Whether the method is static:
            bool methodIsStatic = methodAttributes.Any(s => s.ToLower() == "Static".ToLower());

            //Getting a list of method parameters:
            List<SignatureBaseType> methodParameters = new List<SignatureBaseType>();
            methodParameters.AddRange(signature.ParameterTypes);

            //Getting ALL Method Attributes:
            string methodImplAttributes = methodDef.ImplAttributes.ToString();
            if (!string.IsNullOrEmpty(methodImplAttributes))
            {
                fullMethodAttributes = $"{fullMethodAttributes}, {methodImplAttributes}";
            }

            return new SignatureInfo(@namespace, @class, @methodName, methodAccessibility, methodIsStatic, signature.ReturnType.ToString(), methodParameters, fullMethodAttributes);
        }

        #region GetDecodedSignatures overloads

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var methodDef in methodDefs)
            {
                decodedSignatures.Add(this.GetDecodedSignature(methodDef));
            }

            return decodedSignatures;
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinition typeDef)
        {
            var decodedSignatures = new List<SignatureInfo>();

            decodedSignatures.AddRange(this.GetDecodedSignatures(typeDef.GetMethods().Select(m => this._reader.GetMethodDefinition(m))));

            return decodedSignatures;
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinition> typeDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach(var typeDef in typeDefs)
            {
                decodedSignatures.AddRange(this.GetDecodedSignatures(typeDef));
            }

            return decodedSignatures;
        }

        #endregion

    }
}
