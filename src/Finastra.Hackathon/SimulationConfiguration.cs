using System;
using System.Linq;
using Finastra.Hackathon.Accounting;

namespace Finastra.Hackathon
{
    public static class SimulationConfiguration
    {
        static SimulationConfiguration()
        {
            Reset();
        }

        public static Identity Lender { get; set; }
        public static RatioAnalysis RatioAnalysis { get; set; }
        public static string EmailAddress { get; set; }
        public static Guid SelectedNAIC { get; set; }

        public static bool SimulationStarted { get; set; }
        public static bool CustomerAlteredRationAnalysis { get; set; }
        public static bool LenderProposedAction { get; set; }
        public static bool AlertDismissed { get; set; }

        public static void Reset()
        {
            Lender = StaticData.Lenders.First();
            RatioAnalysis = new RatioAnalysis() { Date = new DateTime(2019, 9, 1), CostOfGoods = 75000, Inventory = 13400, Turnover = 5.60f, TurnDays = 64.32f };
            EmailAddress = "mpool@sycorr.com";
            SelectedNAIC = StaticData.NAIC.Single(x => x.Code == "424490" && x.Name == "Vegetables, canned, merchant wholesalers").Id;

            SimulationStarted = false;
            CustomerAlteredRationAnalysis = false;
            LenderProposedAction = false;
            AlertDismissed = false;
        }
    }
}