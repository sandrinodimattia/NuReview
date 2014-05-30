using System.Web.Mvc;

using NuReview.Services;

namespace NuReview.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReviewService _service;

        public HomeController()
        {
            _service = new ReviewService();
        }

        public ActionResult Index()
        {
            return View(_service.GetReviews());
        }
    }
}