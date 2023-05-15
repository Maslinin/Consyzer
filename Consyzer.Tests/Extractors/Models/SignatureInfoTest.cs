using Xunit;
using System.Reflection;
using System.Collections.Generic;
using Consyzer.Extractors.Models;

namespace Consyzer.Tests.Extractors.Models
{
    public sealed class SignatureInfoTest
    {
        private readonly string _namespace = "MyNamespace";
        private readonly string _class = "MyClass";
        private readonly string _method = "MyMethod";
        private readonly IEnumerable<SignatureParameter> _methodArguments = new List<SignatureParameter>
        {
            new SignatureParameter { Type = "int", Name = "x" },
            new SignatureParameter { Type = "string", Name = "y" }
        };

        [Fact]
        public void MethodLocation_ShouldReturnCorrectLocation()
        {
            var signatureInfo = new SignatureInfo
            {
                Namespace = this._namespace,
                ClassName = this._class,
                MethodName = this._method
            };

            var result = signatureInfo.MethodLocation;

            Assert.Equal($"{this._namespace}.{this._class}.{this._method}", result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FullMethodSignature_ShouldReturnCorrectSignature(bool isStatic)
        {
            var signatureInfo = new SignatureInfo
            {
                Namespace = this._namespace,
                ClassName = this._class,
                MethodName = this._method,
                Accessibility = AccessModifier.Public,
                IsStatic = isStatic,
                ReturnType = new SignatureParameter { Type = "string" },
                MethodArguments = this._methodArguments
            };

            var result = signatureInfo.FullMethodSignature;

            string signature = isStatic ? $"{AccessModifier.Public} {MethodAttributes.Static} string {this._namespace}.{this._class}.{this._method}(int x, string y)" :
                $"{AccessModifier.Public} string {this._namespace}.{this._class}.{this._method}(int x, string y)";

            Assert.Equal(signature, result);
        }

        [Fact]
        public void BaseMethodSignature_ShouldReturnCorrectSignature()
        {
            var signatureInfo = new SignatureInfo
            {
                Namespace = this._namespace,
                ClassName = this._class,
                MethodName = this._method,
                MethodArguments = this._methodArguments
            };

            var result = signatureInfo.BaseMethodSignature;

            Assert.Equal($"{this._namespace}.{this._class}.{this._method}(int x, string y)", result);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectSignature()
        {
            var signatureInfo = new SignatureInfo
            {
                Namespace = this._namespace,
                ClassName = this._class,
                MethodName = this._method,
                MethodArguments = this._methodArguments
            };

            var result = signatureInfo.ToString();

            Assert.Equal($"{this._namespace}.{this._class}.{this._method}(int x, string y)", result);
        }

    }
}
