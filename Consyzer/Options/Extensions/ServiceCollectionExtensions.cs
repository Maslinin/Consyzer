using Consyzer.Reporting;
using Consyzer.Reporting.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace Consyzer.Options.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportWriters(
        this IServiceCollection services,
        AnalysisOptions.OutputFormat formats)
    {
        if (formats.HasFlag(AnalysisOptions.OutputFormat.Console))
            services.AddSingleton<IReportWriter, ConsoleReportWriter>();

        if (formats.HasFlag(AnalysisOptions.OutputFormat.Json))
            services.AddSingleton<IReportWriter, JsonReportWriter>();

        if (formats.HasFlag(AnalysisOptions.OutputFormat.Csv))
            services.AddSingleton<IReportWriter, CsvReportWriter>();

        return services;
    }
}