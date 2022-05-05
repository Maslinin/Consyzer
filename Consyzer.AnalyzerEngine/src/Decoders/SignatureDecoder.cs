using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Consyzer.AnalyzerEngine.CommonModels;
using Consyzer.AnalyzerEngine.Decoders.Providers;
using Consyzer.AnalyzerEngine.Decoders.SyntaxModels;

namespace Consyzer.AnalyzerEngine.Decoders
{
    /// <summary>
    /// [Sealed] Contains tools for decoding a method signature.
    /// </summary>
    public sealed class SignatureDecoder
    {
        /// <summary>
        /// Gets a <b>MetadataReader</b> instance representing the current PE file being processed. 
        /// </summary>
        public MetadataReader MdReader { get; }

        #region SignatureDecoder constructors

        /// <summary>
        /// Initializes a new instance of <b>SignatureDecoder</b>.
        /// </summary>
        /// <param name="binary"></param>
        /// <exception cref="MetadataFileNotSupportedException"></exception>
        /// <exception cref="AssemblyFileNotSupportedException"></exception>
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

        /// <summary>
        /// Initializes a new instance of <b>SignatureDecoder</b>.
        /// </summary>
        /// <param name="pathToBinary"></param>
        public SignatureDecoder(string pathToBinary)
        {
            this.MdReader = new SignatureDecoder(new BinaryFileInfo(pathToBinary)).MdReader;
        }

        /// <summary>
        /// Initializes a new instance of <b>SignatureDecoder</b>.
        /// </summary>
        /// <param name="mdReader"></param>
        public SignatureDecoder(MetadataReader mdReader)
        {
            this.MdReader = mdReader;
        }

        #endregion

        #region GetDecodedSignature overlaods

        /// <summary>
        /// Decodes the method signature and returns a <b>SignatureInfo</b> instance containing the decoded signature information.
        /// </summary>
        /// <param name="methodDef"></param>
        /// <returns>A SignatureInfo instance containing decoded signature information.</returns>
        public SignatureInfo GetDecodedSignature(MethodDefinition methodDef)
        {
            var typeDef = this.MdReader.GetTypeDefinition(methodDef.GetDeclaringType());
            var signature = methodDef.DecodeSignature(new SignatureDecoderTypeProvider(this.MdReader, methodDef), new object());

            //Namespace.Class.MethodName:
            string @namespace = this.MdReader.GetString(typeDef.Namespace), @class = this.MdReader.GetString(typeDef.Name), @methodName = this.MdReader.GetString(methodDef.Name);

            string fullMethodAttributes = methodDef.Attributes.ToString();

            var methodAttributes = fullMethodAttributes.Split(',').Select(s => s.Trim()).ToList();

            AccessibilityModifiers methodAccessibility = (AccessibilityModifiers)(int)Enum.Parse(typeof(AccessibilityModifiersIL), 
                Enum.GetValues(typeof(AccessibilityModifiersIL))
                .OfType<AccessibilityModifiersIL>()
                .Select(m => m.ToString())
                .Intersect(methodAttributes).First());

            bool methodIsStatic = methodAttributes.Any(s => s.ToLower() == "Static");

            List<SignatureBaseType> methodParameters = new List<SignatureBaseType>();
            methodParameters.AddRange(signature.ParameterTypes);

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
                ReturnType = signature.ReturnType,
                MethodArguments = methodParameters,
                MethodAttributes = fullMethodAttributes
            };
        }

        /// <summary>
        /// Decodes the method signature and returns a <b>SignatureInfo</b> instance containing the decoded signature information.
        /// </summary>
        /// <param name="methodHandle"></param>
        /// <returns>A SignatureInfo instance containing decoded signature information.</returns>
        public SignatureInfo GetDecodedSignature(MethodDefinitionHandle methodHandle)
        {
            return this.GetDecodedSignature(this.MdReader.GetMethodDefinition(methodHandle));
        }

        #endregion

        #region GetDecodedSignatures overloads

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeDefs"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinition> typeDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var typeDef in typeDefs)
            {
                decodedSignatures.AddRange(this.GetDecodedSignatures(typeDef));
            }

            return decodedSignatures;
        }

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeDef"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinition typeDef)
        {
            return this.GetDecodedSignatures(typeDef.GetMethods().Select(m => this.MdReader.GetMethodDefinition(m)));
        }

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeHandle"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(TypeDefinitionHandle typeHandle)
        {
            return this.GetDecodedSignatures(this.MdReader.GetTypeDefinition(typeHandle).GetMethods().Select(m => this.MdReader.GetMethodDefinition(m)));
        }

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="typeHadles"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<TypeDefinitionHandle> typeHadles)
        {
            return this.GetDecodedSignatures(typeHadles.Select(h => this.MdReader.GetTypeDefinition(h)));
        }

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="methodDefs"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinition> methodDefs)
        {
            var decodedSignatures = new List<SignatureInfo>();

            foreach (var methodDef in methodDefs)
            {
                decodedSignatures.Add(this.GetDecodedSignature(methodDef));
            }

            return decodedSignatures;
        }

        /// <summary>
        /// Decodes method signatures and returns a collection of <b>SignatureInfo</b> instances containing decoded signature information.
        /// </summary>
        /// <param name="methodHandles"></param>
        /// <returns>A collection of <b>SignatureInfo</b> instances containing decoded signature information.</returns>
        public IEnumerable<SignatureInfo> GetDecodedSignatures(IEnumerable<MethodDefinitionHandle> methodHandles)
        {
            return this.GetDecodedSignatures(methodHandles.Select(h => this.MdReader.GetMethodDefinition(h)));
        }

        #endregion

    }
}
