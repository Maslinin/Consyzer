using System.Reflection;

namespace Consyzer.Core.Models;

internal sealed class PInvokeMethodGroup
{
    public required FileInfo File { get; set; }
    public required IEnumerable<PInvokeMethod> Methods { get; set; }
}

internal sealed class PInvokeMethod
{
    public required MethodSignature Signature { get; init; }
    public required string DllLocation { get; init; }
    public required MethodImportAttributes DllImportFlags { get; init; }
}