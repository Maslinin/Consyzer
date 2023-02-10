namespace Consyzer.Cryptography
{
    public interface IHashInfo
    {
        string MD5Sum { get; }
        string SHA256Sum { get; }
    }
}
