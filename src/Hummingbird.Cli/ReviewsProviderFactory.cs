namespace Hummingbird.Cli
{
    internal class ReviewsProviderFactory : IReviewsProviderFactory
    {
        // TODO: Make robust. Cache created providers.
        public IReviewsProvider GetReviewsProvider(ReviewsSource reviewsSource, string credentials)
        {
            return reviewsSource switch
            {
                ReviewsSource.GitHub => new GitHubReviewsProvider(credentials),
                _ => new GitHubReviewsProvider(credentials)
            };
        }
    }
}
