namespace Consyzer.Cryptography
{
    /// <summary>
    /// Provides an interface for hashing interaction.
    /// </summary>
    public interface IHashInfo
    {
        /// <summary>
        /// Gets the <b>MD5</b> hash sum as a string.
        /// </summary>
        string MD5Sum { get; }
        /// <summary>
        /// Gets the <b>SHA256</b> hash sum as a string.
        /// </summary>
        string SHA256Sum { get; }
    }
}
