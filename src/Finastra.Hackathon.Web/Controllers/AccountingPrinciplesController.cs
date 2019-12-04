using System.Linq;
using System.Net.Mail;
using Finastra.Hackathon.Emails;
using Finastra.Hackathon.Emails.Templates.AlertLender;
using Finastra.Hackathon.ML;
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

            var arePredictionsAbnormal = IsStatisticallyAbnormal();

            if(arePredictionsAbnormal)
                AlertLender();

            return RedirectToAction("Index", "AccountingPrinciples");
        }

        private bool IsStatisticallyAbnormal()
        {
            var predictor = new TimeSeriesPredictor();
            var data = StaticData.AccountingPrinciples.ToList();
            data.Add(SimulationConfiguration.RatioAnalysis);

            var predictions = predictor.Predict(data);
            var year = predictions.Where(x => x.TurnoverDelta > 0).Select(x => x.Date.Year).Min();

            var industryStandard = StaticData.RMA.Where(x => x.Date.Year == year)
                .First(x => x.NAICId == SimulationConfiguration.SelectedNAIC).InventoryTurnover;

            return predictions.Where(x => x.TurnoverDelta > 0).Any(x => x.Turnover > (industryStandard * 1.25));
        }

        private void AlertLender()
        {
            var model = new AlertLenderEmailModel()
            {
                FromAddress = new MailAddress("insights@fbval.com", "Valkyrie Insights"),
                ToAddress = new MailAddress(SimulationConfiguration.EmailAddress,  SimulationConfiguration.Lender.Name + " (Commercial Lending)"),
                Name = SimulationConfiguration.Lender.Name,
                CustomerName = "Peter's Pumpkin Cannery"
            };

            MailMessageFactory.Send(model);
        }
    }
}
