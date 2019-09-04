using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VanillaHttpClient
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

            var client = new HttpClient();

            var content = new StringContent(query);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = authorizationHeader;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Hummingbird", "0.1.0"));

            var response = await client.PostAsync(gitHubGraphQlEndpoint, content);

            Console.WriteLine(response);
        }
    }
}
