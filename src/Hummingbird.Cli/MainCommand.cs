using Hummingbird.Cli.Assets;
using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace Hummingbird.Cli
{
    [Command(Name = "hb", Description = Resources.MainCommand.CommandDescription)]
    [HelpOption(Description = "Show command line help.", Inherited = true)]
    [Subcommand(typeof(TasksCommand))]
    internal class MainCommand
    {
        private readonly CommandLineApplication commandLineApplication;

        public MainCommand(CommandLineApplication commandLineApplication)
        {
            System.Diagnostics.Debug.Assert(commandLineApplication != null);

            this.commandLineApplication = commandLineApplication;
        }

        public Task OnExecute()
        {
            commandLineApplication.ShowHelp(usePager: false);
            return Task.CompletedTask;
        }
    }
}
