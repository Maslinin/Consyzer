﻿using System.Text;
using System.Collections.Generic;

namespace Consyzer.AnalyzerEngine.Signature.Models
{
    /// <summary>
    /// [Sealed] Presents information about the signature of the method and contains tools for displaying it.
    /// </summary>
    public sealed class SignatureInfo
    {
        /// <summary>
        /// Gets the namespace in which the method is declared.
        /// </summary>
        public string Namespace { get; internal set; }
        /// <summary>
        /// Gets the class in which the method is declared.
        /// </summary>
        public string ClassName { get; internal set; }
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string MethodName { get; internal set; }
        /// <summary>
        /// Gets method accessibility modifiers.
        /// </summary>
        public AccessibilityModifier Accessibility { get; internal set; }
        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic { get; internal set; }
        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ISignatureParameterType ReturnType { get; internal set; }
        /// <summary>
        /// Gets method arguments.
        /// </summary>
        public IEnumerable<ISignatureParameterType> MethodArguments { get; internal set; }
        /// <summary>
        /// Gets attributes of the method.
        /// </summary>
        public string MethodAttributes { get; internal set; }
        /// <summary>
        /// Gets attributes of the method implementation.
        /// </summary>
        public string MethodImplAttributes { get; internal set; }

        /// <summary>
        /// Returns the location of the method in the format <b>{Namespace.Class.Method}</b>.
        /// </summary>
        /// <returns>A string containing the location of the method.</returns>
        public string GetMethodLocation()
        {
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}";
        }

        /// <summary>
        /// Returns the complete signature of the method, including the access modifiers and the return value of the method.
        /// </summary>
        /// <returns>A string containing the complete method signature in the format <b>{{Accessibility} {ReturnType} Namespace.Class.Method(Args)}</b>.</returns>
        public string GetFullMethodSignature()
        {
            return $"{this.Accessibility}{(this.IsStatic ? " static" : string.Empty)} {this.ReturnType.Type} {this.GetBaseMethodSignature()})";
        }

        /// <summary>
        /// Returns the base signature of the method defined by the С# language standard.<br/>
        /// Includes ONLY the location of the method and its parameters.
        /// </summary>
        /// <returns>A string containing the base signature of the method in the format <b>{Namespace.Class.Method(Args)}</b>.</returns>
        public string GetBaseMethodSignature()
        {
            return $"{this.Namespace}.{this.ClassName}.{this.MethodName}({this.GetMethodArgsAsString()})";
        }

        /// <summary>
        /// Returns method arguments as a comma-separated string.
        /// </summary>
        /// <returns>String containing method arguments.</returns>
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
        private void AddArgumentAttributeIfItExists(StringBuilder builder, ISignatureParameterType argument)
        {
            if (argument.Attributes != ParametersTypes.AttributesDefaultValue)
            {
                builder.Append($"{argument.Attributes} ");
            }
        }

    }
}