using static Consyzer.Constants.ParameterValue;

namespace Consyzer.Metadata.Models
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class SignatureParameter
    {
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
