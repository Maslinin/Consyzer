namespace Consyzer.Options;

internal sealed class AppOptions
{
    public required ReportOptions Report {  get; init; }

    public sealed class ReportOptions
    {
        public required CsvOptions Csv { get; init; }

        public sealed class CsvOptions
        {
            public required string Delimiter { get; init; }
            public required string? Encoding { get; init; }
        }
    }

}
