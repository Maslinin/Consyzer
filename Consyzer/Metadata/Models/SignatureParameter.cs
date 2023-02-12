namespace Consyzer.Metadata.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureParameter
    {
        public const string AttributesDefaultValue = "None";
        public const string NameDefaultValue = "None";

        public string Type { get; }
        public string Attributes { get; }
        public string Name { get; }


        internal SignatureParameter(string Type, string Attributes = AttributesDefaultValue, string Name = NameDefaultValue)
        {
            this.Type = Type;
            this.Attributes = Attributes;
            this.Name = Name;
        }

    }
}
