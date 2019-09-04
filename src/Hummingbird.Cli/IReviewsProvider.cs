using Hummingbird.Cli.Model;
using System.Collections.Generic;

namespace Hummingbird.Cli
{
    internal interface IReviewsProvider
    {
        IAsyncEnumerable<Review> GetReviews(string repositoryOwnerName, string repositoryName);
    }
}
