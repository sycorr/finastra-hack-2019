using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Finastra.Hackathon.Emails;
using Finastra.Hackathon.ML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Finastra.Hackathon.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Finastra.Hackathon.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Any())
                return RedirectToAction("Logout", "Authorization");

            var isLender = HttpContext.Session.GetString("Role").Equals("Lender", StringComparison.OrdinalIgnoreCase);

            if (isLender)
                return RedirectToAction("Index", "Clients");

            return RedirectToAction("Index", "Accounts");
        }

		//public async Task<IActionResult> Privacy2()
  //      {
  //          var bytes = new HtmlToPDFRenderer().ToPDF(null);
  //          return File(bytes, "application/pdf", "test.pdf");
  //      }
		
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
