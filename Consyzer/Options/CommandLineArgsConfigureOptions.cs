using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Consyzer.Options.Models;

namespace Consyzer.Options;

internal sealed class CommandLineArgsConfigureOptions : IConfigureOptions<CommandLineOptions>
{
    public IConfiguration Configuration { get; }

    public CommandLineArgsConfigureOptions(IConfiguration Configuration)
    {
        this.Configuration = Configuration;
    }

    public void Configure(CommandLineOptions options)
    {
        this.Configuration.Get<CommandLineOptions>();
    }
}