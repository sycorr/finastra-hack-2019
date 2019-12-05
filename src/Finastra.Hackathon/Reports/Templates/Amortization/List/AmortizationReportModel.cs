using System;
using System.Collections.Generic;
using System.Linq;
using Finastra.Hackathon.Finastra;

namespace Finastra.Hackathon.Reports.Templates.Amortization.List
{
    public class AmortizationReportModel : ReportModel
    {
        public AmortizationReportModel()
        {
            DocumentTitle = "Amortization";
            FileName = String.Format("Amortization - {0}", DateTime.Now.ToString("dd_MMM_yyyy"));
            View = "Finastra.Hackathon.Reports.Templates.Amortization.List.AmortizationReport.cshtml";
        }

        public AmoritizationTable AmoritizationTable { get; set; }
    }
}