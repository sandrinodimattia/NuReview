using System;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;

using Microsoft.WindowsAzure.Storage;

namespace NuReview.Controllers
{
    public class MonitoringController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                Trace.TraceInformation("Monitoring request from Traffic Manager.");
                Trace.TraceInformation(" > User Agent: {0}", Request.UserAgent);
                Trace.TraceInformation(" > IP Address: {0}", GetIpAddress());

                // Try to do something with our storage account.
                CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"])
                    .CreateCloudTableClient()
                    .GetTableReference("Reviews")
                    .CreateIfNotExists();

                // Return OK, so TM knows everything is working correctly.
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Trace.TraceError(String.Format("Monitoring request failed: {0}.", ex.Message));

                // Return an error result to notify TM that something is wrong.
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        public string GetIpAddress()
        {
            try
            {
                var request = System.Web.HttpContext.Current.Request;
                var forwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                var removeAddress = request.ServerVariables["REMOTE_ADDR"];
                string ip = "127.0.0.1";
                if (!string.IsNullOrEmpty(forwardedFor) && !forwardedFor.ToLower().Contains("unknown"))
                {
                    forwardedFor = forwardedFor.Trim();
                    string[] ipRange = forwardedFor.Split(',');
                    ip = ipRange[0];
                }
                else if (!string.IsNullOrEmpty(removeAddress))
                {
                    removeAddress = removeAddress.Trim();
                    ip = removeAddress;
                }
                return ip;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}