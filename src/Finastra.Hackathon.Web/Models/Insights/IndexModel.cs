using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finastra.Hackathon.Accounting;
using Finastra.Hackathon.NAICS;

namespace Finastra.Hackathon.Web.Models.Insights
{
    public class IndexModel
    {
        public IEnumerable<RatioAnalysis> RatioAnalyses { get; set; }
        public IEnumerable<RMAInformation> RmaInformations { get; set; }
        public NAICInformation NaicInformation { get; set; }

    }
}
