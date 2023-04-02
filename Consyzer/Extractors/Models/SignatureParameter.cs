namespace Consyzer.Extractors.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureParameter
    {
        public const string AttributesDefaultValue = "None";
        public const string NameDefaultValue = "None";

        public string Type { get; set; }
        public string Attributes { get; set; } = AttributesDefaultValue;
        public string Name { get; set; } = NameDefaultValue;

    }
}
