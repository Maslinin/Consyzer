using Consyzer.Options;
using Consyzer.Reporting;
using Consyzer.Reporting.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace Consyzer.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportWriters(
        this IServiceCollection services,
        AnalysisOptions.OutputFormats formats)
    {
        if (formats.HasFlag(AnalysisOptions.OutputFormats.Console))
            services.AddSingleton<IReportWriter, ConsoleReportWriter>();

        if (formats.HasFlag(AnalysisOptions.OutputFormats.Json))
            services.AddSingleton<IReportWriter, JsonReportWriter>();

        if (formats.HasFlag(AnalysisOptions.OutputFormats.Csv))
            services.AddSingleton<IReportWriter, CsvReportWriter>();

        return services;
    }
}