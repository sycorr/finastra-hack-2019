using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class AccountingPrinciplesController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/AccountingPrinciples/Index.cshtml");
        }
    }
}
