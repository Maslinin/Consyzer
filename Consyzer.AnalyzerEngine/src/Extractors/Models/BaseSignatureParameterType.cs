using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Consyzer.AnalyzerEngine.Tests")]

namespace Consyzer.AnalyzerEngine.Decoders.Models
{
    /// <summary>
    /// [Sealed] Represents a description of a type that is part of a method signature.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class BaseSignatureParameterType : ISignatureParameterType
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

        public string Type { get; }
        public string Attributes { get; }
        public string Name { get; }

        internal BaseSignatureParameterType(string Type, string Attributes = AttributesDefaultValue, string Name = NameDefaultValue)
        {
            this.Type = Type;
            this.Attributes = Attributes;
            this.Name = Name;
        }

    }
}
