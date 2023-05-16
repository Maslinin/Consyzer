using System.Reflection;

namespace Consyzer.Extractors.Models
{
    public enum AccessModifier //for convert msil access modifier names to understandable names
    {
        Private = MethodAttributes.Private,
        Public = MethodAttributes.Public,
        Internal = MethodAttributes.Assembly,
        Protected = MethodAttributes.Family,
        ProtectedInternal = MethodAttributes.FamORAssem,
        ProtectedPrivate = MethodAttributes.FamANDAssem
    }
}
