using System;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace NuReview
{
    public static class Helper
    {
        public static string Location
        {
            get { return ConfigurationManager.AppSettings["Location"]; }
        }
        
        public static bool IsSiteInFailoverMode
        {
            get { return !String.IsNullOrEmpty(ConfigurationManager.AppSettings["Failover"]) 
                && ConfigurationManager.AppSettings["Failover"] != "0"; }
        }

        public static GeoReplicationStats GetReplicationStatus()
        {
            try
            {
                var tableClient = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"])
                    .CreateCloudTableClient();
                tableClient.DefaultRequestOptions.LocationMode = LocationMode.SecondaryOnly;
                return tableClient.GetServiceStats().GeoReplication;
            }
            catch
            {
                return null;
            }
        }

        public static void NotifyError(this Controller controller, string message, params object[] args)
        {
            controller.TempData["ErrorMessage"] = String.Format(message, args);
        }

        public static void NotifySuccess(this Controller controller, string message, params object[] args)
        {
            controller.TempData["SuccessMessage"] = String.Format(message, args);
        }
    }
}