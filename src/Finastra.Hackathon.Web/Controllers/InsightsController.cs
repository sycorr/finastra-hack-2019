using System.Linq;
using Finastra.Hackathon.ML;
using Finastra.Hackathon.Web.Models.Insights;
using Microsoft.AspNetCore.Mvc;
namespace Finastra.Hackathon.Web.Controllers
{
    public class InsightsController : Controller
    {
        public ActionResult Index()
        {
            var predictor = new TimeSeriesPredictor();

            var data = StaticData.AccountingPrinciples.ToList();

            if(SimulationConfiguration.CustomerAlteredRationAnalysis)
                data.Add(SimulationConfiguration.RatioAnalysis);

            var model = new IndexModel();
            model.RatioAnalyses = predictor.Predict(data).OrderBy(x => x.Date);
            model.NaicInformation = StaticData.NAIC.Single(x => x.Id == SimulationConfiguration.SelectedNAIC);
            model.RmaInformations = StaticData.RMA.Where(x => x.NAICId == SimulationConfiguration.SelectedNAIC).OrderBy(x => x.Date);

            return View(model);
        }
    }
}
