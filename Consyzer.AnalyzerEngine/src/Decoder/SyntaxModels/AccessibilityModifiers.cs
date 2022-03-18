using System.Reflection;

namespace Consyzer.AnalyzerEngine.Decoder.SyntaxModels
{
    internal enum AccessibilityModifiersNotTranslated
    {
        Private = MethodAttributes.Private,
        Public = MethodAttributes.Public,
        Assembly = MethodAttributes.Assembly,
        Family = MethodAttributes.Family
    }

    public enum AccessibilityModifiers
    {
        Private = AccessibilityModifiersNotTranslated.Private,
        Public = AccessibilityModifiersNotTranslated.Public,
        Internal = AccessibilityModifiersNotTranslated.Assembly,
        Protected = AccessibilityModifiersNotTranslated.Family
    }
}
