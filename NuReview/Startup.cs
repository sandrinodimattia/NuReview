using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NuReview.Startup))]
namespace NuReview
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
