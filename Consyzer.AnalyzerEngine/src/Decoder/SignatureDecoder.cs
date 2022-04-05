using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.AnalyzerEngine.CommonModels;
using Consyzer.AnalyzerEngine.Decoder.Providers;
using Consyzer.AnalyzerEngine.Decoder.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Decoder
{
    public sealed class SignatureDecoder
    {
        public MetadataReader MdReader { get; }

        #region SignatureDecoder constructors

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public SignatureDecoder(BinaryFileInfo binary)
        {
            if (!binary.HasMetadata)
            {
                throw new MetadataFileNotSupportedException($"{binary.BaseFileInfo.FullName} is does not contain metadata.");
            }
            if (!binary.IsAssembly)
            {
                throw new AssemblyFileNotSupportedException($"{binary.BaseFileInfo.FullName} is contains metadata, but is not an assembly.");
            }

            var peReader = new System.Reflection.PortableExecutable.PEReader(new FileStream(binary.BaseFileInfo.FullName, FileMode.Open, FileAccess.Read));
            this.MdReader = peReader.GetMetadataReader();
        }

        public SignatureDecoder(string pathToBinary)
        {
            this.MdReader = new SignatureDecoder(new BinaryFileInfo(pathToBinary)).MdReader;
        }

        public SignatureDecoder(MetadataReader mdReader)
        {
            this.MdReader = mdReader;
        }

        #endregion

        #region GetDecodedSignature overlaods

        public SignatureInfo GetDecodedSignature(MethodDefinition methodDef)
        {
            var typeDef = this.MdReader.GetTypeDefinition(methodDef.GetDeclaringType());
            var signature = methodDef.DecodeSignature(new SignatureDecoderTypeProvider(this.MdReader, methodDef), new object());

            //Namespace.Class.MethodName:
            string @namespace = this.MdReader.GetString(typeDef.Namespace), @class = this.MdReader.GetString(typeDef.Name), @methodName = this.MdReader.GetString(methodDef.Name);

            string fullMethodAttributes = methodDef.Attributes.ToString();

            //Getting Method Access Modifier:
            var methodAttributes = fullMethodAttributes.Split(',').Select(s => s.Trim()).ToList();
            string methodAccessibility = string.Empty;

            foreach (AccessibilityModifiersNotTranslated modifier in Enum.GetValues(typeof(AccessibilityModifiersNotTranslated)))
            { //does not take into account private protected and protected internal
                int indexOf = methodAttributes.IndexOf(methodAttributes.Find(s => s.ToLower() == modifier.ToString().ToLower()));
                if (indexOf != -1)
                {
                    methodAccessibility = ((AccessibilityModifiers)Enum.GetValues(typeof(AccessibilityModifiers)).GetValue(indexOf)).ToString();
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

            return new SignatureInfo()
            {
                Namespace = @namespace,
                ClassName = @class,
                MethodName = methodName,
                Accessibility = methodAccessibility,
                IsStatic = methodIsStatic,
                ReturnType = signature.ReturnType.ToString(),
                MethodArguments = methodParameters,
                AllMethodAttributes = fullMethodAttributes
            };
        }

        public SignatureInfo GetDecodedSignature(MethodDefinitionHandle methodHandle)
        {
            return this.GetDecodedSignature(this.MdReader.GetMethodDefinition(methodHandle));
        }

        #endregion

        #region GetDecodedSignatures overloads

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
            return this.GetDecodedSignatures(typeDef.GetMethods().Select(m => this.MdReader.GetMethodDefinition(m)));
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinitionHandle typeHandle)
        {
            return this.GetDecodedSignatures(this.MdReader.GetTypeDefinition(typeHandle).GetMethods().Select(m => this.MdReader.GetMethodDefinition(m)));
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinitionHandle> typeHadles)
        {
            return this.GetDecodedSignatures(typeHadles.Select(h => this.MdReader.GetTypeDefinition(h)));
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var methodDef in methodDefs)
            {
                decodedSignatures.Add(this.GetDecodedSignature(methodDef));
            }

            return decodedSignatures;
        }

        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinitionHandle> methodHandles)
        {
            return this.GetDecodedSignatures(methodHandles.Select(h => this.MdReader.GetMethodDefinition(h)));
        }

        #endregion

    }
}
