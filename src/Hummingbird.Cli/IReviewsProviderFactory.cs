namespace Hummingbird.Cli
{
    internal interface IReviewsProviderFactory
    {
        IReviewsProvider GetReviewsProvider(ReviewsSource reviewsSource, string credentials);
    }
}
