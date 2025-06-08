namespace Consyzer.Core.Models;

internal sealed class MethodSignature
{
    public required string ReturnType { get; init; }
    public required bool IsStatic { get; init; }
    public required string Namespace { get; init; }
    public required string Class { get; init; }
    public required string Method { get; init; }
    public required IEnumerable<string> MethodArguments { get; init; }

    public override string ToString()
    {
        string separatedArgs = string.Join(", ", MethodArguments);
        string isStatic = IsStatic ? " static" : string.Empty;
        return $"{ReturnType}{isStatic} {GetMethodLocation()}({separatedArgs})";
    }

    public string GetMethodLocation()
    {
        return $"{Namespace}.{Class}.{Method}";
    }
}