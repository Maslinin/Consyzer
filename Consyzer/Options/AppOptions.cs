namespace Consyzer.Options;

internal sealed class AppOptions
{
    public required ReportOptions Report {  get; set; }

    public sealed class ReportOptions
    {
        public required CsvOptions Csv { get; set; }

        public sealed class CsvOptions
        {
            public required string Encoding { get; set; }
            public required char Delimiter { get; set; }
        }

        public required XmlOptions Xml { get; set; }

        public sealed class XmlOptions
        {
            public required string Encoding { get; set; }
            public required string IndentChars { get; set; }
        }
    }
}