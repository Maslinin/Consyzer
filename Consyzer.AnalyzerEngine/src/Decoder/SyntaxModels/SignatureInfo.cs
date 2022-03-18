using System.Text;
using System.Collections.Generic;
using Consyzer.AnalyzerEngine.Decoder.Provider;

namespace Consyzer.AnalyzerEngine.Decoder.SyntaxModels
{
    public sealed class SignatureInfo
    {
        public string Namespace { get; }
        public string ClassName { get; }
        public string MethodName { get; }
        public string Accessibility { get; }
        public bool IsStatic { get; }
        public string ReturnType { get; }
        public IEnumerable<SignatureBaseType> Parameters { get; }
        public string AllMethodAttributes { get; }

        public SignatureInfo(string Namespace,
                             string ClassName,
                             string MethodName,
                             string Accessibility,
                             bool IsStatic,
                             string ReturnType,
                             IEnumerable<SignatureBaseType> Parameters,
                             string AllMethodAttributes)
        {
            this.Namespace = Namespace;
            this.ClassName = ClassName;
            this.MethodName = MethodName;
            this.Accessibility = Accessibility;
            this.IsStatic = IsStatic;
            this.ReturnType = ReturnType;
            this.Parameters = Parameters;
            this.AllMethodAttributes = AllMethodAttributes;
        }

        public string GetMethodLocation()
        {
            return $"{Namespace}.{ClassName}.{MethodName}";
        }

        public string GetMethodParametersAsString()
        {
            var builder = new StringBuilder();

            foreach (var parameter in this.Parameters)
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
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}({this.GetMethodParametersAsString()})";
        }

        public string GetFullMethodSignature()
        {
            return $"{this.Accessibility} {(this.IsStatic ? "static" : string.Empty)} {this.ReturnType} {this.GetBaseMethodSignature()})";
        }
    }
}
