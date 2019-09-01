using System.Threading.Tasks;

namespace Hummingbird.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await CommandLineApplicationExecutor.Execute<MainCommand>(args);
        }
    }
}
