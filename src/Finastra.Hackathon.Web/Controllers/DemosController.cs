using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class DemosController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Demos.cshtml");
        }
    }
}
