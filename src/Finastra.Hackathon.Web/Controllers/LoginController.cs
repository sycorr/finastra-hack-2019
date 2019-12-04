using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home/Login.cshtml");
        }
    }
}
