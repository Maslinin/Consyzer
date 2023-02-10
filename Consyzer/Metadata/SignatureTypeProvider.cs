using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Consyzer.Metadata.Models;
using static Consyzer.Constants.ParameterType;
using static Consyzer.Constants.ParameterValue;

namespace Consyzer.Metadata
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureTypeProvider : ISignatureTypeProvider<SignatureParameter, object>
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

        public SignatureParameter GetByReferenceType(SignatureParameter elementType)
        {
            return new SignatureParameter($"{elementType.Type}&", elementType.Attributes, elementType.Name);
        }

        public SignatureParameter GetFunctionPointerType(MethodSignature<SignatureParameter> signature)
        {
            var type = new StringBuilder();

            type.Append($"{signature.ReturnType} *({signature.ParameterTypes[0].Type}");
            for (int i = 1; i < signature.ParameterTypes.Length; ++i)
            {
                type.Append($", {signature.ParameterTypes[i].Type}");
            }
            type.Append(')');

            return new SignatureParameter(type.ToString());
        }

        public SignatureParameter GetGenericInstantiation(SignatureParameter genericType, ImmutableArray<SignatureParameter> typeArguments)
        {
            var type = new StringBuilder();

            type.Append($"{genericType.Type} <{typeArguments[0].Type}");
            for (int i = 1; i < typeArguments.Length; ++i)
            {
                type.Append($", {typeArguments[i].Type}");
            }
            type.Append('>');

            return new SignatureParameter(type.ToString(), genericType.Attributes, genericType.Name);
        }

        public SignatureParameter GetModifiedType(SignatureParameter modifier, SignatureParameter unmodifiedType, bool isRequired)
        {
            return unmodifiedType;
        }

        public SignatureParameter GetPointerType(SignatureParameter elementType)
        {
            return new SignatureParameter($"{elementType.Type}*", elementType.Attributes, elementType.Name);
        }

        public SignatureParameter GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string attributes = AttributesDefaultValue, name = NameDefaultValue;

            if (!this._isReturnType) //Always return method type is first defined
            {
                this._isReturnType = true;
            }
            else
            {
                var parameterHandle = this._methodDef.GetParameters().ElementAtOrDefault(this._parameterIteration);
                var parameter = this._mdReader.GetParameter(parameterHandle);

                attributes = parameter.Attributes.ToString() ?? AttributesDefaultValue;
                name = this._mdReader.GetString(parameter.Name) ?? NameDefaultValue;

                ++this._parameterIteration;
            }

            return GetParameter(typeCode, attributes, name);
        }

        public SignatureParameter GetSZArrayType(SignatureParameter elementType)
        {
            return new SignatureParameter($"{elementType.Type}[]", elementType.Attributes, elementType.Name);
        }

        public SignatureParameter GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            var typeSpecification = reader.GetTypeSpecification(handle);
            var decodedSignature = typeSpecification.DecodeSignature(this, genericContext);

            return decodedSignature;
        }

        #region NotSupported

        //.NET only support SZAray
        public SignatureParameter GetArrayType(SignatureParameter elementType, ArrayShape shape)
        {
            return new SignatureParameter(NotSupported);
        }

        public SignatureParameter GetGenericMethodParameter(object genericContext, int index)
        {
            return new SignatureParameter(NotSupported);
        }

        public SignatureParameter GetGenericTypeParameter(object genericContext, int index)
        {
            return new SignatureParameter(NotSupported);
        }

        public SignatureParameter GetPinnedType(SignatureParameter elementType)
        {
            return new SignatureParameter(NotSupported);
        }

        public SignatureParameter GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new SignatureParameter(NotSupported);
        }

        public SignatureParameter GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new SignatureParameter(NotSupported);
        }

        #endregion

        #region Helper Methods

        private static SignatureParameter GetParameter(PrimitiveTypeCode typeCode, string attributes, string name)
        {
            return typeCode switch
            {
                PrimitiveTypeCode.Boolean => new SignatureParameter(Boolean, attributes, name),
                PrimitiveTypeCode.Byte => new SignatureParameter(Byte, attributes, name),
                PrimitiveTypeCode.SByte => new SignatureParameter(SByte, attributes, name),
                PrimitiveTypeCode.Char => new SignatureParameter(Char, attributes, name),
                PrimitiveTypeCode.Int16 => new SignatureParameter(Short, attributes, name),
                PrimitiveTypeCode.UInt16 => new SignatureParameter(UShort, attributes, name),
                PrimitiveTypeCode.Int32 => new SignatureParameter(Int, attributes, name),
                PrimitiveTypeCode.UInt32 => new SignatureParameter(UInt, attributes, name),
                PrimitiveTypeCode.Int64 => new SignatureParameter(Long, attributes, name),
                PrimitiveTypeCode.UInt64 => new SignatureParameter(ULong, attributes, name),
                PrimitiveTypeCode.Single => new SignatureParameter(Float, attributes, name),
                PrimitiveTypeCode.Double => new SignatureParameter(Double, attributes, name),
                PrimitiveTypeCode.IntPtr => new SignatureParameter(IntPtr, attributes, name),
                PrimitiveTypeCode.UIntPtr => new SignatureParameter(UIntPtr, attributes, name),
                PrimitiveTypeCode.String => new SignatureParameter(String, attributes, name),
                PrimitiveTypeCode.Object => new SignatureParameter(Object, attributes, name),
                PrimitiveTypeCode.TypedReference => new SignatureParameter(TypedReference, attributes, name),
                PrimitiveTypeCode.Void => new SignatureParameter(Void, attributes, name),
                _ => new SignatureParameter(NotSupported, attributes, name),
            };
        }

        #endregion
    }
}