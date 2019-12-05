using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class StartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reset()
        {
            SimulationConfiguration.Reset();
            return RedirectToAction("Index", "Start");
        }

        [HttpPost]
        public ActionResult Set(string username, string email, Guid naic)
        {
            SimulationConfiguration.EmailAddress = email;
            SimulationConfiguration.Lender = StaticData.Lenders.Single(x => x.Username == username);
            SimulationConfiguration.SelectedNAIC = naic;

            SimulationConfiguration.SimulationStarted = true;
            SimulationConfiguration.CustomerAlteredRationAnalysis = false;
            SimulationConfiguration.LenderProposedAction = false;
            SimulationConfiguration.AlertDismissed = false;

            SimulationConfiguration.ProposedAmoritizationTable = null;

            return RedirectToAction("Login", "Authorization");
        }

        [HttpGet]
        public ActionResult SetDismissed()
        {
            SimulationConfiguration.AlertDismissed = true;
            return Ok();
        }
    }
}
