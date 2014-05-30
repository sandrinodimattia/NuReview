using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace NuReview.Services
{
    public class ReviewService
    {
        private CloudTable GetTable()
        {
            var tableClient = CloudStorageAccount
                .Parse(ConfigurationManager.AppSettings["StorageConnectionString"])
                .CreateCloudTableClient();

            // In failover mode, we don't want the overhead to connect to the primary storage account first.
            if (Helper.IsSiteInFailoverMode)
                tableClient.DefaultRequestOptions.LocationMode = LocationMode.SecondaryOnly;


            return tableClient.GetTableReference("Reviews");
        }

        public void SubmitReview(string name, string packageId, string body, int score)
        {
            if (Helper.IsSiteInFailoverMode)
            {
                throw new Exception("We're sorry, but writing new reviews is currently not possible");
            }

            // Persist to table storage.
            GetTable().Execute(TableOperation.Insert(new Review
            {
                Name = name,
                Body = body,
                PackageId = packageId,
                CreatedOn = DateTimeOffset.UtcNow,
                PartitionKey =  "0" + (DateTimeOffset.MaxValue.Ticks - DateTimeOffset.UtcNow.Date.Ticks),
                RowKey = "0" + (DateTimeOffset.MaxValue.Ticks - DateTimeOffset.UtcNow.Ticks) + "+" + Guid.NewGuid(),
                Score = score
            }));
        }

        public IEnumerable<Review> GetReviews()
        {
            return GetTable().ExecuteQuery(new TableQuery<Review>().Take(50));
        }
    }
}