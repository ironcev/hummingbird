namespace Hummingbird.Cli.Model
{
    internal class ReviewTask
    {
        public ReviewTaskStatus Status { get; }

        public string Description { get; }

        public ReviewTask(ReviewTaskStatus status, string description)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(description));

            Status = status;
            Description = description;
        }
    }
}
