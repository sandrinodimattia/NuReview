using System;
using System.Web;
using System.Web.Mvc;
using NuReview.Models;
using NuReview.Services;

namespace NuReview.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewService _service;

        public ReviewController()
        {
            _service = new ReviewService();
        }

        public ActionResult Write()
        {
            return View(new ReviewEditModel());
        }

        [HttpPost]
        public ActionResult Write(ReviewEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.SubmitReview(model.Name, model.PackageId, model.Body, model.Score);

                    // Success!
                    this.NotifySuccess("Your review about <strong>{0}</strong> has been added.", HttpUtility.HtmlEncode(model.PackageId));
                }
                catch (Exception ex)
                {
                    // Failed!
                    this.NotifyError(ex.Message);
                }

                // Back to homepage.
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}