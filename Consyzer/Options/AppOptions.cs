namespace Consyzer.Options;

internal sealed class AppOptions
{
    public required ReportOptions Report {  get; set; }

    public sealed class ReportOptions
    {
        public required CsvOptions Csv { get; set; }

        public sealed class CsvOptions
        {
            public required string Delimiter { get; set; }
            public required string? Encoding { get; set; }
        }
    }
}