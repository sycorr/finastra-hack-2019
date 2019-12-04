using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class AccountingPrinciplesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Set(float costOfGoods, float inventory)
        {
            SimulationConfiguration.RatioAnalysis.CostOfGoods = costOfGoods;
            SimulationConfiguration.RatioAnalysis.Inventory = inventory;
            SimulationConfiguration.CustomerAlteredRationAnalysis = true;

            return RedirectToAction("Index", "AccountingPrinciples");
        }
    }
}
