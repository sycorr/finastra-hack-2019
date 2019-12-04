using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class SimResetController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Partials/SimReset.cshtml");
        }
    }
}
