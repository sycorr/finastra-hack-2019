using System.Threading.Tasks;
using Finastra.Hackathon.Platform;
using Finastra.Hackathon.Reports.Templates.Amortization.List;
using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Loan()
        {
            var model = new AmortizationReportModel()
            {
                AmoritizationTable = SimulationConfiguration.ProposedAmoritizationTable
            };

            var bytes = model.ToPDF();

            return File(bytes, "application/pdf", "Proposed_Loan_Adjustment.pdf");
        }
    }
}
