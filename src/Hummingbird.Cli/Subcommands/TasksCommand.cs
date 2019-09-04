using Hummingbird.Abstractions;
using Hummingbird.Cli.Assets;
using Hummingbird.Cli.Model;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hummingbird.Cli
{
    [Command(Name = "tasks", Description = Resources.TasksCommand.CommandDescription)]
    internal class TasksCommand : BaseCommand
    {
        private readonly IOutputSink output;
        private readonly IReviewsProviderFactory reviewsProviderFactory;

        [Option("-u|--user", Description = Resources.TasksCommand.UserDescription)]
        [Required]

        // TODO: Add validators.
        public string User { get; private set; } = string.Empty;

        public TasksCommand(IOutputSink output, IReviewsProviderFactory reviewsProviderFactory)
        {
            System.Diagnostics.Debug.Assert(output != null);
            System.Diagnostics.Debug.Assert(reviewsProviderFactory != null);

            this.output = output;
            this.reviewsProviderFactory = reviewsProviderFactory;
        }

        public async Task OnExecute()
        {
            WriteDoubleUnderscoredHeading($"Tasks for {User}");

            var reviewsProvider = reviewsProviderFactory.GetReviewsProvider(ReviewsSource.GitHub, Credentials);

            var repositoryOwnerAndName = Repository.Split(':');
            var reviews = reviewsProvider.GetReviews(repositoryOwnerAndName[0], repositoryOwnerAndName[1]);

            var toAct = new List<Review>();
            var toClose = new List<Review>();
            await foreach (var review in reviews)
            {
                if (review.IsToActBy(User)) toAct.Add(review);
                if (review.IsToCloseBy(User)) toClose.Add(review);
            }

            OutputReviews(toAct, "To act");
            OutputReviews(toClose, "To close");

            void OutputReviews(IReadOnlyCollection<Review> reviewsToOutput, string heading)
            {
                if (reviewsToOutput.Count <= 0) return;

                WriteDoubleUnderscoredHeading(heading);

                foreach (var reviewByPath in reviewsToOutput.GroupBy(review => review.Path))
                {
                    output.WriteMessage(reviewByPath.Key);

                    foreach (var reviewInfo in reviewByPath.OrderBy(review => review.Url))
                    {
                        output.WriteMessage($"  {reviewInfo.Url}");
                        foreach (var task in reviewInfo.Tasks)
                            output.WriteMessage($"    {GetTaskDescription(task.Description)}");
                    }

                    output.WriteLine();
                }
            }

            string GetTaskDescription(string originalDescription)
            {
                // TODO: Remove magic number. Respect console buffer size.
                if (originalDescription.Length < 72) return originalDescription;

                return originalDescription.Substring(0, 72 - 3) + "...";
            }

            void WriteDoubleUnderscoredHeading(string message)
            {
                output.WriteMessage(message);
                output.WriteMessage(new string('=', message.Length));
                output.WriteLine();
            }
        }
    }
}
