using System.Reflection.Metadata;

namespace Consyzer.AnalyzerEngine.Signature.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class ParametersTypes
    {
        #region Types Defenitions

        internal static string NotSupported => "!notsupported";
        internal static string Boolean => "bool";
        internal static string Byte => "byte";
        internal static string SByte => "sbyte";
        internal static string Char => "char";
        internal static string Short => "short";
        internal static string UShort => "ushort";
        internal static string Int => "int";
        internal static string UInt => "uint";
        internal static string Long => "long";
        internal static string ULong => "ulong";
        internal static string Float => "float";
        internal static string Double => "double";
        internal static string IntPtr => "IntPtr";
        internal static string UIntPtr => "UIntPtr";
        internal static string String => "string";
        internal static string Object => "object";
        internal static string TypedReference => "typedref";
        internal static string Void => "void";

        #endregion

        #region Constants

        internal const string AttributesDefaultValue = "None";
        internal const string NameDefaultValue = "None";

        #endregion

        public static ISignatureParameterType GetSignatureParameterType(PrimitiveTypeCode typeCode, string attributes, string name)
        {
            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean: 
                    return new BaseSignatureParameterType(Boolean, attributes, name);
                case PrimitiveTypeCode.Byte: 
                    return new BaseSignatureParameterType(Byte, attributes, name);
                case PrimitiveTypeCode.SByte: 
                    return new BaseSignatureParameterType(SByte, attributes, name);
                case PrimitiveTypeCode.Char: 
                    return new BaseSignatureParameterType(Char, attributes, name);
                case PrimitiveTypeCode.Int16: 
                    return new BaseSignatureParameterType(Short, attributes, name);
                case PrimitiveTypeCode.UInt16: 
                    return new BaseSignatureParameterType(UShort, attributes, name);
                case PrimitiveTypeCode.Int32: 
                    return new BaseSignatureParameterType(Int, attributes, name);
                case PrimitiveTypeCode.UInt32: 
                    return new BaseSignatureParameterType(UInt, attributes, name);
                case PrimitiveTypeCode.Int64: 
                    return new BaseSignatureParameterType(Long, attributes, name);
                case PrimitiveTypeCode.UInt64: 
                    return new BaseSignatureParameterType(ULong, attributes, name);
                case PrimitiveTypeCode.Single: 
                    return new BaseSignatureParameterType(Float, attributes, name);
                case PrimitiveTypeCode.Double: 
                    return new BaseSignatureParameterType(Double, attributes, name);
                case PrimitiveTypeCode.IntPtr: 
                    return new BaseSignatureParameterType(IntPtr, attributes, name);
                case PrimitiveTypeCode.UIntPtr: 
                    return new BaseSignatureParameterType(UIntPtr, attributes, name);
                case PrimitiveTypeCode.String: 
                    return new BaseSignatureParameterType(String, attributes, name);
                case PrimitiveTypeCode.Object: 
                    return new BaseSignatureParameterType(Object, attributes, name);
                case PrimitiveTypeCode.TypedReference: 
                    return new BaseSignatureParameterType(TypedReference, attributes, name);
                case PrimitiveTypeCode.Void: 
                    return new BaseSignatureParameterType(Void, attributes, name);
                default: 
                    return new BaseSignatureParameterType(NotSupported, attributes, name);
            }
        }

    }
}
