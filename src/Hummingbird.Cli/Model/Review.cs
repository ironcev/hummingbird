using System;
using System.Collections.Generic;
using System.Linq;

namespace Hummingbird.Cli.Model
{
    internal class Review
    {
        public string Reviewer { get; }

        public string? Committer { get; }

        public string Url { get; }

        public string Path { get; }

        public IReadOnlyCollection<ReviewTask> Tasks { get; }

        public ReviewStatus Status { get; }

        internal Review(string reviewer, string? committer, string url, string path, IReadOnlyCollection<ReviewTask> tasks)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(reviewer));
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(url));
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(path));
            System.Diagnostics.Debug.Assert(tasks != null && tasks.Count != 0);

            Reviewer = reviewer;
            Committer = committer;
            Url = url;
            Path = path;
            Tasks = tasks;

            Status = GetReviewStatus();

            ReviewStatus GetReviewStatus()
            {
                var numberOfOpenTasks = tasks.Count(task => task.Status == ReviewTaskStatus.Open);

                if (numberOfOpenTasks == 0) return ReviewStatus.ToBeClosed;
                if (numberOfOpenTasks == tasks.Count) return ReviewStatus.Open;

                return ReviewStatus.InWork;
            }
        }

        public bool IsToActBy(string committerUsername)
        {
            return Status != ReviewStatus.ToBeClosed && string.Equals(Committer, committerUsername, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsToCloseBy(string reviewerUsername)
        {
            return Status == ReviewStatus.ToBeClosed && string.Equals(Reviewer, reviewerUsername, StringComparison.OrdinalIgnoreCase);
        }
    }
}
