namespace Consyzer.AnalyzerEngine.Decoder.Providers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureBaseType
    {
        #region Type Defenitions

        public static string NotSupported => "!notsupported";
        public static string Boolean => "bool";
        public static string Byte => "byte";
        public static string SByte => "sbyte";
        public static string Char => "char";
        public static string Short => "short";
        public static string UShort => "ushort";
        public static string Int => "int";
        public static string UInt => "uint";
        public static string Long => "long";
        public static string ULong => "ulong";
        public static string Float => "float";
        public static string Double => "double";
        public static string IntPtr => "IntPtr";
        public static string UIntPtr => "UIntPtr";
        public static string String => "string";
        public static string Object => "object";
        public static string TypedReference => "typedref";
        public static string Void => "void";

        #endregion

        public const string AttributesDefaultValue = "None";
        public const string NameDefaultValue = "None";

        public string Type { get; }
        public string Attributes { get; }
        public string Name { get; }

        public SignatureBaseType(string Type, string Attributes = SignatureBaseType.AttributesDefaultValue, string Name = SignatureBaseType.NameDefaultValue)
        {
            this.Type = Type;
            this.Attributes = Attributes;
            this.Name = Name;
        }

        public bool IsPrimitive => this.ToString() != Void && this.ToString() != TypedReference && this.ToString() != NotSupported;

        public override string ToString()
        {
            return this.Type;
        }
    }
}
