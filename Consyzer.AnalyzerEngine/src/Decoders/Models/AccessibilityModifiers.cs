using System.Reflection;

namespace Consyzer.AnalyzerEngine.Decoders.Models
{
    internal enum MsilAccessibilityModifiers
    {
        Private = MethodAttributes.Private,
        Public = MethodAttributes.Public,
        Assembly = MethodAttributes.Assembly,
        Family = MethodAttributes.Family,
        FamORAssem = MethodAttributes.FamORAssem,
        FamANDAssem = MethodAttributes.FamANDAssem
    }

    /// <summary>
    /// Represents the values for the method access modifier definition (corresponds to MSDN MethodAttributes).
    /// </summary>
    public enum AccessibilityModifiers
    {
        Private = MsilAccessibilityModifiers.Private,
        Public = MsilAccessibilityModifiers.Public,
        Internal = MsilAccessibilityModifiers.Assembly,
        Protected = MsilAccessibilityModifiers.Family,
        ProtectedInternal = MsilAccessibilityModifiers.FamORAssem,
        PrivateProtected = MsilAccessibilityModifiers.FamANDAssem
    }
}
