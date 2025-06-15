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

        return services;
    }
}