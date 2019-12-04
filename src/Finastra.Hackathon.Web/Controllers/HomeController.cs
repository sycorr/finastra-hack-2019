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
using Finastra.Hackathon.Web.Tasks;

namespace Finastra.Hackathon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var predictor = new TimeSeriesPredictor();
            var model = predictor.GetValues();

            return View(model);
        }

        public async Task<IActionResult> Privacy()
        {
            var email = "mpool@sycorr.com";
            await new EmailTasks().SendAlert(email);

            return View();
        }

		public async Task<IActionResult> Privacy2()
        {
            var bytes = new HtmlToPDFRenderer().ToPDF(null);
            return File(bytes, "application/pdf", "test.pdf");
        }
		
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
