using System.Text;
using System.Collections.Generic;

namespace Consyzer.Metadata.Models
{
    internal sealed class SignatureInfo
    {
        public string Namespace { get; internal set; }
        public string ClassName { get; internal set; }
        public string MethodName { get; internal set; }
        public AccessModifier Accessibility { get; internal set; }
        public bool IsStatic { get; internal set; }
        public SignatureParameter ReturnType { get; internal set; }
        public IEnumerable<SignatureParameter> MethodArguments { get; internal set; }
        public string MethodAttributes { get; internal set; }
        public string MethodImplAttributes { get; internal set; }

        public string GetMethodLocation()
        {
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}";
        }

        public string GetFullMethodSignature()
        {
            return $"{this.Accessibility}{(this.IsStatic ? " static" : string.Empty)} {this.ReturnType.Type} {this.GetBaseMethodSignature()})";
        }

        public string GetBaseMethodSignature()
        {
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}({this.GetMethodArgsAsString()})";
        }

        public string GetMethodArgsAsString()
        {
            var builder = new StringBuilder();

            foreach (var argument in this.MethodArguments)
            {
                this.AddArgumentAttributeIfItExists(builder, argument);

                builder.Append($"{argument.Type} {argument.Name}, ");
            }
            if (builder.Length != 0) //Remove the comma and space at the end of the line
            {
                builder.Remove(builder.Length - 2, 2);
            }

            return builder.ToString();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private void AddArgumentAttributeIfItExists(StringBuilder builder, SignatureParameter argument)
        {
            if (argument.Attributes != SignatureParameter.AttributesDefaultValue)
            {
                builder.Append($"{argument.Attributes} ");
            }
        }

    }
}
