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
        public ActionResult Set(string username, string email)
        {
            SimulationConfiguration.EmailAddress = email;
            SimulationConfiguration.Lender = StaticData.Lenders.Single(x => x.Username == username);
            SimulationConfiguration.SimulationStarted = true;

            return RedirectToAction("Login", "Authorization");
        }
    }
}
