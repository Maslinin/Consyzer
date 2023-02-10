using System.Reflection;

namespace Consyzer.Metadata.Models
{
    internal enum MsilAccessModifier
    {
        Private = MethodAttributes.Private,
        Public = MethodAttributes.Public,
        Assembly = MethodAttributes.Assembly,
        Family = MethodAttributes.Family,
        FamORAssem = MethodAttributes.FamORAssem,
        FamANDAssem = MethodAttributes.FamANDAssem
    }

    internal enum AccessModifier
    {
        Private = MsilAccessModifier.Private,
        Public = MsilAccessModifier.Public,
        Internal = MsilAccessModifier.Assembly,
        Protected = MsilAccessModifier.Family,
        ProtectedInternal = MsilAccessModifier.FamORAssem,
        ProtectedPrivate = MsilAccessModifier.FamANDAssem
    }
}
