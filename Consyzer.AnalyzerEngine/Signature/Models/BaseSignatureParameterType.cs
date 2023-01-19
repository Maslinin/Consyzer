using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Consyzer.AnalyzerEngine.Tests")]

namespace Consyzer.AnalyzerEngine.Signature.Models
{
    /// <summary>
    /// [Sealed] Represents a description of a type that is part of a method signature.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class BaseSignatureParameterType : ISignatureParameterType
    {
        public string Type { get; }
        public string Attributes { get; }
        public string Name { get; }

        internal BaseSignatureParameterType(string Type, string Attributes = ParametersTypes.AttributesDefaultValue, string Name = ParametersTypes.NameDefaultValue)
        {
            this.Type = Type;
            this.Attributes = Attributes;
            this.Name = Name;
        }

    }
}
