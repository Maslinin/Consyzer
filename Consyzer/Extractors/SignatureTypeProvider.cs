using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Consyzer.Signature.Models;

namespace Consyzer.Signature
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureTypeProvider : ISignatureTypeProvider<ISignatureParameterType, object>
    {
        private readonly MetadataReader _mdReader;
        private readonly MethodDefinition _methodDef;

        //status fields:
        private bool _isReturnType = false;
        private int _parameterIteration = 0;

        public SignatureTypeProvider(MetadataReader mdReader, MethodDefinition methodDef)
        {
            this._mdReader = mdReader;
            this._methodDef = methodDef;
        }

        public ISignatureParameterType GetByReferenceType(ISignatureParameterType elementType)
        {
            return new BaseSignatureParameterType($"{elementType.Type}&", elementType.Attributes, elementType.Name);
        }

        public ISignatureParameterType GetFunctionPointerType(MethodSignature<ISignatureParameterType> signature)
        {
            var type = new StringBuilder();

            type.Append($"{signature.ReturnType} *({signature.ParameterTypes[0].Type}");
            for (int i = 1; i < signature.ParameterTypes.Length; ++i)
            {
                type.Append($", {signature.ParameterTypes[i].Type}");
            }
            type.Append(')');

            return new BaseSignatureParameterType(type.ToString());
        }

        public ISignatureParameterType GetGenericInstantiation(ISignatureParameterType genericType, ImmutableArray<ISignatureParameterType> typeArguments)
        {
            var type = new StringBuilder();

            type.Append($"{genericType.Type} <{typeArguments[0].Type}");
            for (int i = 1; i < typeArguments.Length; ++i)
            {
                type.Append($", {typeArguments[i].Type}");
            }
            type.Append('>');

            return new BaseSignatureParameterType(type.ToString(), genericType.Attributes, genericType.Name);
        }

        public ISignatureParameterType GetModifiedType(ISignatureParameterType modifier, ISignatureParameterType unmodifiedType, bool isRequired)
        {
            return unmodifiedType;
        }

        public ISignatureParameterType GetPointerType(ISignatureParameterType elementType)
        {
            return new BaseSignatureParameterType($"{elementType.Type}*", elementType.Attributes, elementType.Name);
        }

        public ISignatureParameterType GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string attributes = ParametersTypes.AttributesDefaultValue, name = ParametersTypes.NameDefaultValue;

            if (!this._isReturnType) //Always return method type is first defined
            {
                this._isReturnType = true;
            }
            else
            {
                var parameterHandle = this._methodDef.GetParameters().ElementAtOrDefault(this._parameterIteration);
                var parameter = this._mdReader.GetParameter(parameterHandle);

                attributes = parameter.Attributes.ToString() ?? ParametersTypes.AttributesDefaultValue;
                name = this._mdReader.GetString(parameter.Name) ?? ParametersTypes.NameDefaultValue;

                ++this._parameterIteration;
            }

            return ParametersTypes.GetSignatureParameterType(typeCode, attributes, name);
        }

        public ISignatureParameterType GetSZArrayType(ISignatureParameterType elementType)
        {
            return new BaseSignatureParameterType($"{elementType.Type}[]", elementType.Attributes, elementType.Name);
        }

        public ISignatureParameterType GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            var typeSpecification = reader.GetTypeSpecification(handle);
            var decodedSignature = typeSpecification.DecodeSignature(this, genericContext);

            return decodedSignature;
        }

        #region NotSupported

        //.NET only support SZAray
        public ISignatureParameterType GetArrayType(ISignatureParameterType elementType, ArrayShape shape)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        public ISignatureParameterType GetGenericMethodParameter(object genericContext, int index)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        public ISignatureParameterType GetGenericTypeParameter(object genericContext, int index)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        public ISignatureParameterType GetPinnedType(ISignatureParameterType elementType)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        public ISignatureParameterType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        public ISignatureParameterType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new BaseSignatureParameterType(ParametersTypes.NotSupported);
        }

        #endregion

    }
}