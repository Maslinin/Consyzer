using System.Reflection;

namespace Consyzer.Signature.Models
{
    internal enum MsilAccessibilityModifier
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
    public enum AccessibilityModifier
    {
        Private = MsilAccessibilityModifier.Private,
        Public = MsilAccessibilityModifier.Public,
        Internal = MsilAccessibilityModifier.Assembly,
        Protected = MsilAccessibilityModifier.Family,
        ProtectedInternal = MsilAccessibilityModifier.FamORAssem,
        PrivateProtected = MsilAccessibilityModifier.FamANDAssem
    }
}
