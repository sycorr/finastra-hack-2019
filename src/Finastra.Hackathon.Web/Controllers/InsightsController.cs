using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class InsightsController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Insights/Index.cshtml");
        }
    }
}
