namespace Consyzer.AnalyzerEngine.Decoders.Providers
{
    /// <summary>
    /// Represents the base implementation of a class implementing a type representation in a decoded method signature.
    /// </summary>
    public interface ISignatureType
    {
        /// <summary>
        /// Gets a string representation of the type of the current instance.
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Gets a string representation of the attributes of the current instance.
        /// </summary>
        string Attributes { get; }
        /// <summary>
        /// Gets the string representation of the type variable name represented by the current instance.
        /// </summary>
        string Name { get; }
    }
}
