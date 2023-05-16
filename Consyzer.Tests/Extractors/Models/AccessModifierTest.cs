using Xunit;
using System.Reflection;
using Consyzer.Extractors.Models;

namespace Consyzer.Tests.Extractors.Models
{
    public sealed class AccessModifierTest
    {
        [Theory]
        [InlineData(AccessModifier.Private, MethodAttributes.Private)]
        [InlineData(AccessModifier.Public, MethodAttributes.Public)]
        [InlineData(AccessModifier.Internal, MethodAttributes.Assembly)]
        [InlineData(AccessModifier.Protected, MethodAttributes.Family)]
        [InlineData(AccessModifier.ProtectedInternal, MethodAttributes.FamORAssem)]
        [InlineData(AccessModifier.ProtectedPrivate, MethodAttributes.FamANDAssem)]
        public void AccessModifier_ShouldHaveCorrectValue(AccessModifier accessModifier, MethodAttributes methodAttribute)
        {
            int expectedValue = (int)methodAttribute;

            int actualValue = (int)accessModifier;

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
