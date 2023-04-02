using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Consyzer.Extractors.Models
{
    internal sealed class SignatureInfo
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public AccessModifier Accessibility { get; set; }
        public bool IsStatic { get; set; }
        public SignatureParameter ReturnType { get; set; }
        public IEnumerable<SignatureParameter> MethodArguments { get; set; }

        public string MethodLocation =>
            $"{this.Namespace}.{this.ClassName}.{this.MethodName}";

        public string FullMethodSignature => 
            $"{this.Accessibility}{(this.IsStatic ? $" {nameof(MethodAttributes.Static)}" : string.Empty)} {this.ReturnType.Type} {this.BaseMethodSignature})";

        public string BaseMethodSignature =>
            $"{this.Namespace}.{this.ClassName}.{this.MethodName}({string.Join(", ", this.MethodArguments.Select(this.GetArgumentString))})";

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string GetArgumentString(SignatureParameter argument) =>
            $"{(argument.Attributes != SignatureParameter.AttributesDefaultValue ? $"{argument.Attributes} " : string.Empty)}{argument.Type} {argument.Name}";

        public override string ToString() => BaseMethodSignature;
    }
}
