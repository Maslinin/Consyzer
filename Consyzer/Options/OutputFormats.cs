namespace Consyzer.Options;

[Flags]
public enum OutputFormats
{
    Console = 1 << 0,
    Csv = 1 << 1,
    Json = 1 << 2
}