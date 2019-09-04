using Hummingbird.Cli.Extensions;
using Hummingbird.Cli.Model;
using Octokit.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hummingbird.Cli
{
    internal class GitHubReviewsProvider : IReviewsProvider
    {
        private class CommitComment
        {
            public string Author { get; set; } = string.Empty;

            public string? Committer { get; set; }

            public string Body { get; set; } = string.Empty;

            public string Url { get; set; } = string.Empty;

            public string Path { get; set; } = string.Empty;

            public DateTimeOffset CreatedAt { get; set; }

            public bool IsReview
            {
                get
                {
                    // We expect IsReview to be called only once in the Release build.
                    // Still, a cheap heuristic lazy loading.
                    if (numberOfOpenTasks > 0 || numberOfClosedTasks > 0) return true;

                    (numberOfOpenTasks, numberOfClosedTasks) = GetNumberOfOpenedAndClosedTasks(Body);

                    return numberOfOpenTasks > 0 || numberOfClosedTasks > 0;

                    static (int, int) GetNumberOfOpenedAndClosedTasks(string body)
                    {
                        return (body.NumberOfOccurrencesOfSubstring("- [ ]"), body.NumberOfOccurrencesOfSubstring("- [x]"));
                    }
                }
            }

            private int numberOfOpenTasks;
            private int numberOfClosedTasks;

            public Review ToReview()
            {
                System.Diagnostics.Debug.Assert(IsReview);

                return new Review
                (
                    Author,
                    Committer,
                    Url.Replace("#commitcomment-", "#r"),
                    Path,
                    GetReviewTasks().ToArray()
                );

                IEnumerable<ReviewTask> GetReviewTasks()
                {
                    foreach (var line in Body.Split("\r\n"))
                    {
                        var text = line.AsSpan().Trim();
                        if (text.StartsWith("- [ ]"))
                            yield return new ReviewTask(ReviewTaskStatus.Open, text.ToString());
                        else if (text.StartsWith("-  [x]"))
                            yield return new ReviewTask(ReviewTaskStatus.Closed, text.ToString());
                    }
                }
            }
        }

        private readonly Connection connection;

        public GitHubReviewsProvider(string accessToken)
        {
            // TODO: Checks that the access token is a properly formed access token.
            // TODO: Get the product name and version from the assembly.
            connection = new Connection(new ProductHeaderValue("Hummingbird", "0.1.0"), accessToken);
        }

        async IAsyncEnumerable<Review> IReviewsProvider.GetReviews(string repositoryOwnerUsername, string repositoryName)
        {
            var query = new Query()
                .Repository(repositoryName, repositoryOwnerUsername)
                .CommitComments()
                .AllPages()
                .Select(commitComment => new CommitComment
                {
                    Author = commitComment.Author.Login,
                    Committer = commitComment.Commit.Committer.User.Login,
                    Body = commitComment.Body,
                    Url = commitComment.Url,
                    Path = commitComment.Path,
                    CreatedAt = commitComment.CreatedAt
                })
                .Compile();

            var result = await connection.Run(query);

            foreach (var reviewComment in result.Where(comment => comment.IsReview))
            {
                yield return reviewComment.ToReview();
            }
        }
    }
}
