using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureParameterTypeProvider : ISignatureTypeProvider<SignatureParameter, object>
    {
        private readonly MetadataReader _mdReader;
        private readonly MethodDefinition _methodDef;

        //status fields:
        private bool _isReturnType = false;
        private int _parameterIteration = 0;

        public SignatureParameterTypeProvider(MetadataReader mdReader, MethodDefinition methodDef)
        {
            this._mdReader = mdReader;
            this._methodDef = methodDef;
        }

        public SignatureParameter GetByReferenceType(SignatureParameter elementType)
        {
            return new SignatureParameter
            {
                Type = $"{elementType.Type}&",
                Attributes = elementType.Attributes,
                Name = elementType.Name
            };
        }

        public SignatureParameter GetFunctionPointerType(MethodSignature<SignatureParameter> signature)
        {
            var type = new StringBuilder()
                .Append($"{signature.ReturnType} *({signature.ParameterTypes[0].Type}")
                .AppendJoin(", ", signature.ParameterTypes.Skip(1).Select(p => p.Type))
                .Append(")");

            return new SignatureParameter { Type = type.ToString() };
        }

        public SignatureParameter GetGenericInstantiation(SignatureParameter genericType, ImmutableArray<SignatureParameter> typeArguments)
        {
            var type = new StringBuilder()
                .Append($"{genericType.Type} <{typeArguments[0].Type}")
                .AppendJoin(", ", typeArguments.Skip(1).Select(p => p.Type))
                .Append(">");

            return new SignatureParameter
            {
                Type = type.ToString(),
                Attributes = genericType.Attributes,
                Name = genericType.Name
            };
        }

        public SignatureParameter GetModifiedType(SignatureParameter modifier, SignatureParameter unmodifiedType, bool isRequired)
        {
            return unmodifiedType;
        }

        public SignatureParameter GetPointerType(SignatureParameter elementType)
        {
            return new SignatureParameter
            {
                Type = $"{elementType.Type}*",
                Attributes = elementType.Attributes,
                Name = elementType.Name
            };
        }

        public SignatureParameter GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string attributes = SignatureParameter.AttributesDefaultValue, name = SignatureParameter.NameDefaultValue;

            if(this._isReturnType)
            {
                var parameterHandle = this._methodDef.GetParameters().ElementAtOrDefault(this._parameterIteration);
                var parameter = this._mdReader.GetParameter(parameterHandle);

                attributes = parameter.Attributes.ToString() ?? SignatureParameter.AttributesDefaultValue;
                name = this._mdReader.GetString(parameter.Name) ?? SignatureParameter.NameDefaultValue;

                ++this._parameterIteration;
            }
            else //Always return method type is first defined
            {
                this._isReturnType = true;
            }

            return new SignatureParameter
            {
                Type = typeCode.ToString(),
                Attributes = attributes,
                Name = name
            };
        }

        public SignatureParameter GetSZArrayType(SignatureParameter elementType)
        {
            return new SignatureParameter
            {
                Type = $"{elementType.Type}[]",
                Attributes = elementType.Attributes,
                Name = elementType.Name
            };
        }

        public SignatureParameter GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            var typeSpecification = reader.GetTypeSpecification(handle);
            var decodedSignature = typeSpecification.DecodeSignature(this, genericContext);

            return decodedSignature;
        }

        #region NotSupported

        private const string NotSupported = "!notsupported";

        //.NET only support SZAray
        public SignatureParameter GetArrayType(SignatureParameter elementType, ArrayShape shape)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        public SignatureParameter GetGenericMethodParameter(object genericContext, int index)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        public SignatureParameter GetGenericTypeParameter(object genericContext, int index)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        public SignatureParameter GetPinnedType(SignatureParameter elementType)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        public SignatureParameter GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        public SignatureParameter GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new SignatureParameter { Type = NotSupported };
        }

        #endregion

    }
}