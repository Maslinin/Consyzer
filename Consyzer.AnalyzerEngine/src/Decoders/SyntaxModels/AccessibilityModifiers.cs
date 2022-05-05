using System;
using System.Reflection;

namespace Consyzer.AnalyzerEngine.Decoders.SyntaxModels
{
    internal enum AccessibilityModifiersIL
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
    [Flags]
    public enum AccessibilityModifiers
    {
        Private = AccessibilityModifiersIL.Private,
        Public = AccessibilityModifiersIL.Public,
        Internal = AccessibilityModifiersIL.Assembly,
        Protected = AccessibilityModifiersIL.Family,
        ProtectedInternal = AccessibilityModifiersIL.FamORAssem,
        PrivateProtected = AccessibilityModifiersIL.FamANDAssem
    }
}
