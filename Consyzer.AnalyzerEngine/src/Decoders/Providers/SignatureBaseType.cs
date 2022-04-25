namespace Consyzer.AnalyzerEngine.Decoders.Providers
{
    /// <summary>
    /// [Sealed] Represents a description of a type that is part of a method signature.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class SignatureBaseType
    {
        #region Type Defenitions

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

        /// <summary>
        /// Gets a string representation of the type of the current instance.
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Gets a string representation of the attributes of the current instance.
        /// </summary>
        public string Attributes { get; }
        /// <summary>
        /// Gets the string representation of the type variable name represented by the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets true if the current instance is a primitive type representation; otherwise false.
        /// </summary>
        public bool IsPrimitive => this.ToString() != Void && this.ToString() != TypedReference && this.ToString() != NotSupported;

        /// <summary>
        /// Initializes a new instance of <b>SignatureBaseType</b>.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Attributes"></param>
        /// <param name="Name"></param>
        internal SignatureBaseType(string Type, string Attributes = SignatureBaseType.AttributesDefaultValue, string Name = SignatureBaseType.NameDefaultValue)
        {
            this.Type = Type;
            this.Attributes = Attributes;
            this.Name = Name;
        }

        /// <summary>
        /// Converts the current instance to a string representation of the type represented by the current instance.
        /// </summary>
        /// <returns>String representation of the type represented by the current instance.</returns>
        public override string ToString()
        {
            return this.Type;
        }
    }
}
