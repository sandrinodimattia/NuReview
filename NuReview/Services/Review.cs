using System;

using Microsoft.WindowsAzure.Storage.Table;

namespace NuReview.Services
{
    public class Review : TableEntity
    {
        public DateTimeOffset CreatedOn
        {
            get;
            set;
        }

        public string PackageId
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }
        
        public int Score
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}