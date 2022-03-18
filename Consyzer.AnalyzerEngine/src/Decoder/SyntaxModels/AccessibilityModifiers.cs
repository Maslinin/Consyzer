using System.Reflection;

namespace Consyzer.AnalyzerEngine.Decoder.SyntaxModels
{
    internal enum AccessibilityModifiersIL
    {
        Private = MethodAttributes.Private,
        Public = MethodAttributes.Public,
        Assembly = MethodAttributes.Assembly,
        Family = MethodAttributes.Family
    }

    public enum AccessibilityModifiers
    {
        Private = AccessibilityModifiersIL.Private,
        Public = AccessibilityModifiersIL.Public,
        Internal = AccessibilityModifiersIL.Assembly,
        Protected = AccessibilityModifiersIL.Family
    }
}
