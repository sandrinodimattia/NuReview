using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.WindowsAzure.Storage;

namespace NuReview
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                if (!Helper.IsSiteInFailoverMode)
                {
                    var tableClient = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"])
                        .CreateCloudTableClient();
                    var table = tableClient.GetTableReference("Reviews");
                    table.CreateIfNotExists();
                }
            }
            catch
            {
                
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
