using System.Collections.Generic;

namespace Consyzer.Extractors.Models;

internal sealed class SignatureInfo
{
    public string Namespace { get; set; }
    public string Class { get; set; }
    public string Method { get; set; }
    public IEnumerable<string> MethodArguments { get; set; }
}