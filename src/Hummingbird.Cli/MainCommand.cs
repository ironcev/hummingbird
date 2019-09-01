using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace Hummingbird.Cli
{
    [Command(Name = "hb", Description = "A lightweight command line tool for GitHub commit comment based code reviews.")]
    [HelpOption(Description = "Show command line help.", Inherited = true)]
    internal partial class MainCommand
    {
        public Task OnExecute(CommandLineApplication app)
        {
            app.ShowHelp(usePager: false);

            return Task.CompletedTask;
        }
    }
}
