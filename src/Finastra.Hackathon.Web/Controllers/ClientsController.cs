using System;
using Microsoft.AspNetCore.Mvc;

namespace Finastra.Hackathon.Web.Controllers
{
    public class ClientsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Insights()
        {
            return View();
        }
    }
}