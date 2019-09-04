using GraphQL.Client;
using GraphQL.Common.Request;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDotNetClient
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

            var client = new GraphQLClient(gitHubGraphQlEndpoint);
            client.DefaultRequestHeaders.Authorization = authorizationHeader;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hummingbird", "0.1.0"));

            var request = new GraphQLRequest
            {
                Query = query
            };

            var response = await client.PostAsync(request);

            Console.WriteLine($"Hello {response.Data.name}!");
        }
    }
}
