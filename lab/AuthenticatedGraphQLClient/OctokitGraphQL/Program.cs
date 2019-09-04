using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Octokit.GraphQL;

namespace OctokitGraphQL
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // -- Common code start --
            const string gitHubGraphQlEndpoint = "http://api.github.com/graphql";

            string credentials;
            if (args.Length == 1)
            {
                credentials = args[0];
            }
            else
            {
                Console.Write("Enter credentials (<username>:<password> or <personal access token>): ");
                credentials = Console.ReadLine();
            }

            AuthenticationHeaderValue authorizationHeader;
            if (credentials.Contains(":"))
            {
                authorizationHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials)));
            }
            else
            {
                authorizationHeader = new AuthenticationHeaderValue("Bearer", credentials);
            }

            var query = @"query {{
                          viewer {{
                            login
                            name
                          }}
                        }}";
            // -- Common code end --

            var productInformation = new Octokit.GraphQL.ProductHeaderValue("Hummingbird", "0.1.0");
            var connection = new Connection(productInformation, credentials);

            var octokitQuery = new Query()
                .Viewer
                .Select(viewer => new
                {
                    viewer.Login,
                    viewer.Name
                })
                .Compile();

            var result = await connection.Run(octokitQuery);

            Console.WriteLine($"Hello {result.Name}!");
        }
    }
}
