using System.Linq;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Immutable;
using Consyzer.AnalyzerEngine.Decoders.Models;

namespace Consyzer.AnalyzerEngine.Decoders
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureParameterTypeProvider : ISignatureTypeProvider<ISignatureParameterType, object>
    {
        private readonly MetadataReader _mdReader;
        private readonly MethodDefinition _methodDef;

        //status fields:
        private bool _isReturnType = false;
        private int _parameterIteration = 0;

        public SignatureParameterTypeProvider(MetadataReader mdReader, MethodDefinition methodDef)
        {
            _mdReader = mdReader;
            _methodDef = methodDef;
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
            string attributes = BaseSignatureParameterType.AttributesDefaultValue, name = BaseSignatureParameterType.NameDefaultValue;

            if (!_isReturnType) //Always return method type is first defined
            {
                _isReturnType = true;
            }
            else
            {
                var parameterHandle = _methodDef.GetParameters().ElementAtOrDefault(_parameterIteration);
                var parameter = this._mdReader.GetParameter(parameterHandle);

                attributes = parameter.Attributes.ToString() ?? BaseSignatureParameterType.AttributesDefaultValue;
                name = this._mdReader.GetString(parameter.Name) ?? BaseSignatureParameterType.NameDefaultValue;

                ++_parameterIteration;
            }

            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean: return new BaseSignatureParameterType(BaseSignatureParameterType.Boolean, attributes, name);
                case PrimitiveTypeCode.Byte: return new BaseSignatureParameterType(BaseSignatureParameterType.Byte, attributes, name);
                case PrimitiveTypeCode.SByte: return new BaseSignatureParameterType(BaseSignatureParameterType.SByte, attributes, name);
                case PrimitiveTypeCode.Char: return new BaseSignatureParameterType(BaseSignatureParameterType.Char, attributes, name);
                case PrimitiveTypeCode.Int16: return new BaseSignatureParameterType(BaseSignatureParameterType.Short, attributes, name);
                case PrimitiveTypeCode.UInt16: return new BaseSignatureParameterType(BaseSignatureParameterType.UShort, attributes, name);
                case PrimitiveTypeCode.Int32: return new BaseSignatureParameterType(BaseSignatureParameterType.Int, attributes, name);
                case PrimitiveTypeCode.UInt32: return new BaseSignatureParameterType(BaseSignatureParameterType.UInt, attributes, name);
                case PrimitiveTypeCode.Int64: return new BaseSignatureParameterType(BaseSignatureParameterType.Long, attributes, name);
                case PrimitiveTypeCode.UInt64: return new BaseSignatureParameterType(BaseSignatureParameterType.ULong, attributes, name);
                case PrimitiveTypeCode.Single: return new BaseSignatureParameterType(BaseSignatureParameterType.Float, attributes, name);
                case PrimitiveTypeCode.Double: return new BaseSignatureParameterType(BaseSignatureParameterType.Double, attributes, name);
                case PrimitiveTypeCode.IntPtr: return new BaseSignatureParameterType(BaseSignatureParameterType.IntPtr, attributes, name);
                case PrimitiveTypeCode.UIntPtr: return new BaseSignatureParameterType(BaseSignatureParameterType.UIntPtr, attributes, name);
                case PrimitiveTypeCode.String: return new BaseSignatureParameterType(BaseSignatureParameterType.String, attributes, name);
                case PrimitiveTypeCode.Object: return new BaseSignatureParameterType(BaseSignatureParameterType.Object, attributes, name);
                case PrimitiveTypeCode.TypedReference: return new BaseSignatureParameterType(BaseSignatureParameterType.TypedReference, attributes, name);
                case PrimitiveTypeCode.Void: return new BaseSignatureParameterType(BaseSignatureParameterType.Void, attributes, name);
                default: return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported, attributes, name);
            }
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
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        public ISignatureParameterType GetGenericMethodParameter(object genericContext, int index)
        {
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        public ISignatureParameterType GetGenericTypeParameter(object genericContext, int index)
        {
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        public ISignatureParameterType GetPinnedType(ISignatureParameterType elementType)
        {
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        public ISignatureParameterType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        public ISignatureParameterType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            return new BaseSignatureParameterType(BaseSignatureParameterType.NotSupported);
        }

        #endregion

    }
}