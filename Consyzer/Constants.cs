namespace Consyzer
{
    internal static class Constants
    {
        public enum ProgramStatusCode
        {
            UnexpectedBehavior = -1,
            SuccessExit = 0
        }

        public enum FileExistanceStatusCode
        {
            FileExistsAtAnalysisPath,
            FileExistsAtAbsolutePath,
            FileExistsAtRelativePath,
            FileExistsAtSystemFolder,
            FileDoesNotExists
        }

        public static class ParameterType
        {
            public const string NotSupported = "!notsupported";
            public const string Boolean = "bool";
            public const string Byte = "byte";
            public const string SByte = "sbyte";
            public const string Char = "char";
            public const string Short = "short";
            public const string UShort = "ushort";
            public const string Int = "int";
            public const string UInt = "uint";
            public const string Long = "long";
            public const string ULong = "ulong";
            public const string Float = "float";
            public const string Double = "double";
            public const string IntPtr = "IntPtr";
            public const string UIntPtr = "UIntPtr";
            public const string String = "string";
            public const string Object = "object";
            public const string Void = "void";
            public const string TypedReference = "typedref";
        }

        public static class ParameterValue
        {
            public const string AttributesDefaultValue = "None";
            public const string NameDefaultValue = "None";
        }
    }
}
