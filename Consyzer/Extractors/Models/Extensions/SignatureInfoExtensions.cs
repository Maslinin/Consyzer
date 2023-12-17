namespace Consyzer.Extractors.Models.Extensions;

internal static class SignatureInfoExtensions
{
    public static string GetMethodSignature(this SignatureInfo signatureInfo)
    {
        return $"{signatureInfo.GetMethodLocation()}({string.Join(", ", signatureInfo.MethodArguments)})";
    }

    public static string GetMethodLocation(this SignatureInfo signatureInfo)
    {
        return $"{signatureInfo.Namespace}.{signatureInfo.Class}.{signatureInfo.Method}";
    }
}