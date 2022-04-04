using System.Text;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Decoder.Providers;

namespace Consyzer.AnalyzerEngine.Decoder.SyntaxModels
{
    public sealed class SignatureInfo
    {
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Accessibility { get; set; }
        public bool IsStatic { get; set; }
        public string ReturnType { get; set; }
        public IEnumerable<SignatureBaseType> MethodArguments { get; set; }
        public string AllMethodAttributes { get; set; }

        public SignatureInfo()
        {
            this.Namespace = string.Empty;
            this.ClassName = string.Empty;
            this.MethodName = string.Empty;
            this.Accessibility = string.Empty;
            this.ReturnType = string.Empty;
            this.MethodArguments = new List<SignatureBaseType>();
            this.AllMethodAttributes = string.Empty;
        }

        public string GetMethodLocation()
        {
            return $"{Namespace}.{ClassName}.{MethodName}";
        }

        public string GetMethodArgsAsString()
        {
            var builder = new StringBuilder();

            foreach (var parameter in this.MethodArguments)
            {
                if (parameter.Attributes != SignatureBaseType.AttributesDefaultValue)
                {
                    builder.Append($"{parameter.Attributes} ");
                }

                builder.Append($"{parameter.Type} {parameter.Name}, ");
            }
            if (builder.Length != 0) //Remove the comma and space at the end of the line
            {
                builder.Remove(builder.Length - 2, 2);
            }

            return builder.ToString();
        }

        public string GetBaseMethodSignature()
        {
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}({this.GetMethodArgsAsString()})";
        }

        public string GetFullMethodSignature()
        {
            return $"{this.Accessibility} {(this.IsStatic ? "static" : string.Empty)} {this.ReturnType} {this.GetBaseMethodSignature()})";
        }
    }
}
