using Hummingbird.Cli.Assets;
using McMaster.Extensions.CommandLineUtils;

namespace Hummingbird.Cli
{
    internal abstract class BaseCommand
    {
        public const string VerboseLongName = "verbose";
        public const string VerboseLongOption = "--" + VerboseLongName;
        public const string VerboseShortOption = "-v";

        [Option(VerboseShortOption + "|" + VerboseLongOption, Description = Resources.BaseCommand.VerboseDescription)]
        public bool Verbose { get; private set; }

        public const string NoColorLongName = "no-color";
        public const string NoColorLongOption = "--" + NoColorLongName;
        public const string NoColorShortOption = "-nc";

        [Option(NoColorShortOption + "|" + NoColorLongOption, Description = Resources.BaseCommand.NoColorDescription)]
        public bool NoColor { get; private set; }
    }
}