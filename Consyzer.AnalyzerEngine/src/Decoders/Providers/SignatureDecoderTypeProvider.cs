using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Immutable;

namespace Consyzer.AnalyzerEngine.Decoders.Providers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureDecoderTypeProvider : ISignatureTypeProvider<SignatureBaseType, object>
    {
        private readonly MetadataReader _mdReader;
        private readonly MethodDefinition _methodDef;

        //status fields:
        private bool _isReturnType = false;
        private int _parameterIteration = 0;

        public SignatureDecoderTypeProvider(MetadataReader mdReader, MethodDefinition methodDef) 
        {
            this._mdReader = mdReader;
            this._methodDef = methodDef;
        }

        public SignatureBaseType GetByReferenceType(SignatureBaseType elementType)
        {
            return new SignatureBaseType($"{elementType.Type}&", elementType.Attributes, elementType.Name);
        }

        public SignatureBaseType GetFunctionPointerType(MethodSignature<SignatureBaseType> signature)
        {
            var type = new StringBuilder();

            type.Append($"{signature.ReturnType} *({signature.ParameterTypes[0].Type}");
            for (int i = 1; i < signature.ParameterTypes.Length; i++)
            {
                type.Append($", {signature.ParameterTypes[i].Type}");
            }
            type.Append(')');

            return new SignatureBaseType(type.ToString());
        }

        public SignatureBaseType GetGenericInstantiation(SignatureBaseType genericType, ImmutableArray<SignatureBaseType> typeArguments)
        {
            var type = new StringBuilder();
            type.Append($"{genericType.Type} <{typeArguments[0].Type}");
            for (int i = 1; i < typeArguments.Length; i++)
            {
                type.Append($", {typeArguments[i].Type}");
            }
            type.Append('>');

            return new SignatureBaseType(type.ToString(), genericType.Attributes, genericType.Name);
        }

        public SignatureBaseType GetModifiedType(SignatureBaseType modifier, SignatureBaseType unmodifiedType, bool isRequired)
        {
            return unmodifiedType;
        }

        public SignatureBaseType GetPointerType(SignatureBaseType elementType)
        {
            return new SignatureBaseType($"{elementType.Type}*", elementType.Attributes, elementType.Name);
        }

        public SignatureBaseType GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string attributes = SignatureBaseType.AttributesDefaultValue, name = SignatureBaseType.NameDefaultValue;

            //Always return method type is first defined
            if (!this._isReturnType)
            {
                this._isReturnType = true;
            }
            else
            {
                Parameter parameter = this._mdReader.GetParameter(this._methodDef.GetParameters().ElementAtOrDefault(this._parameterIteration));
                attributes = parameter.Attributes.ToString() ?? SignatureBaseType.AttributesDefaultValue;
                name = this._mdReader.GetString(parameter.Name) ?? SignatureBaseType.NameDefaultValue;

                ++this._parameterIteration;
            }

            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean: return new SignatureBaseType(SignatureBaseType.Boolean, attributes, name);
                case PrimitiveTypeCode.Byte: return new SignatureBaseType(SignatureBaseType.Byte, attributes, name);
                case PrimitiveTypeCode.SByte: return new SignatureBaseType(SignatureBaseType.SByte, attributes, name);
                case PrimitiveTypeCode.Char: return new SignatureBaseType(SignatureBaseType.Char, attributes, name);
                case PrimitiveTypeCode.Int16: return new SignatureBaseType(SignatureBaseType.Short, attributes, name);
                case PrimitiveTypeCode.UInt16: return new SignatureBaseType(SignatureBaseType.UShort, attributes, name);
                case PrimitiveTypeCode.Int32: return new SignatureBaseType(SignatureBaseType.Int, attributes, name);
                case PrimitiveTypeCode.UInt32: return new SignatureBaseType(SignatureBaseType.UInt, attributes, name);
                case PrimitiveTypeCode.Int64: return new SignatureBaseType(SignatureBaseType.Long, attributes, name);
                case PrimitiveTypeCode.UInt64: return new SignatureBaseType(SignatureBaseType.ULong, attributes, name);
                case PrimitiveTypeCode.Single: return new SignatureBaseType(SignatureBaseType.Float, attributes, name);
                case PrimitiveTypeCode.Double: return new SignatureBaseType(SignatureBaseType.Double, attributes, name);
                case PrimitiveTypeCode.IntPtr: return new SignatureBaseType(SignatureBaseType.IntPtr, attributes, name);
                case PrimitiveTypeCode.UIntPtr: return new SignatureBaseType(SignatureBaseType.UIntPtr, attributes, name);
                case PrimitiveTypeCode.String: return new SignatureBaseType(SignatureBaseType.String, attributes, name);
                case PrimitiveTypeCode.Object: return new SignatureBaseType(SignatureBaseType.Object, attributes, name);
                case PrimitiveTypeCode.TypedReference: return new SignatureBaseType(SignatureBaseType.TypedReference, attributes, name);
                case PrimitiveTypeCode.Void: return new SignatureBaseType(SignatureBaseType.Void, attributes, name);
                default: return new SignatureBaseType(SignatureBaseType.NotSupported, attributes, name);
            }
        }

        public SignatureBaseType GetSZArrayType(SignatureBaseType elementType)
        {
            return new SignatureBaseType($"{elementType.Type}[]", elementType.Attributes, elementType.Name);
        }

        public SignatureBaseType GetTypeFromSpecification(MetadataReader reader, object genericContext, TypeSpecificationHandle handle, byte rawTypeKind)
        {
            return reader.GetTypeSpecification(handle).DecodeSignature(this, genericContext);
        }

        #region NotSupported

        //.NET we only support SZAray
        public SignatureBaseType GetArrayType(SignatureBaseType elementType, ArrayShape shape)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        public SignatureBaseType GetGenericMethodParameter(object genericContext, int index)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        public SignatureBaseType GetGenericTypeParameter(object genericContext, int index)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        public SignatureBaseType GetPinnedType(SignatureBaseType elementType)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        public SignatureBaseType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        public SignatureBaseType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new SignatureBaseType(SignatureBaseType.NotSupported);
        }

        #endregion

    }
}