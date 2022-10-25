using System.Reflection;

namespace Consyzer.AnalyzerEngine.Decoders.Models
{
    internal enum CILAccessibilityModifiers
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
        Private = CILAccessibilityModifiers.Private,
        Public = CILAccessibilityModifiers.Public,
        Internal = CILAccessibilityModifiers.Assembly,
        Protected = CILAccessibilityModifiers.Family,
        ProtectedInternal = CILAccessibilityModifiers.FamORAssem,
        PrivateProtected = CILAccessibilityModifiers.FamANDAssem
    }
}
