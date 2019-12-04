using System;
using System.Collections.Generic;
using System.Linq;
using Finastra.Hackathon.Reports.Excel;

namespace Finastra.Hackathon.Reports.Templates.Reviews.Overview
{
    public class OverviewModel : ReportModel
    {
        private ApplicationOverviewByGranularity _total;
        private Review _review;

        public OverviewModel()
        {
            Applications = new List<ApplicationOverview>();
            DocumentTitle = "Review Summary Report";
        }

        public Review Review
        {
            get { return _review; }
            set
            {
                _review = value;
                FileName = String.Format("Permission Assist - Review Summary - {1} - {0}", DateTime.UtcNow.ToCompanyTime("dd_MMM_yyyy"), _review.Name);
            }
        }

        public string CompanyName { get; set; }
        public int TotalIdentities { get; set; }
        public int TotalRoles { get; set; }
        public int TotalAutoApproved { get; set; }
        public DateTime? IdentitiesSince { get; set; }
        public List<ApplicationOverview> Applications { get; set; }

        public string HowManyPass
        {
            get
            {
                var i = 0;

                if (Review.AreApplicationOwnerReviewsRequired)
                    i++;

                if (Review.AreSecurityTeamReviewsRequired)
                    i++;

                if (Review.AreSupervisorReviewsRequired)
                    i++;

                if (i == 0)
                    return "zero";
                
                if (i == 1)
                    return "single";

                if (i == 2)
                    return "double";

                return "triple";
            }
        }

        public ApplicationOverviewByGranularity Total 
        {
            get
            {
                if (_total == null)
                    _total = new ApplicationOverviewByGranularity()
                    {
                        PendingItems = Applications.Sum(x => x.Total.PendingItems),
                        ApprovedItems = Applications.Sum(x => x.Total.ApprovedItems),
                        RemediatedItems = Applications.Sum(x => x.Total.RemediatedItems),
                        FlaggedItems = Applications.Sum(x => x.Total.FlaggedItems),
                    };

                return _total;
            }
        }

        public class ApplicationOverview
        {
            public Guid? ApplicationId { get; set; }
            public Guid? SegregationRuleId { get; set; }
            public Guid? ApplicationImportId { get; set; }
            public string Name { get; set; }
            public DateTime? AsOfDate { get; set; }
            public ApplicationOverviewByGranularity Total { get; set; }
            public ImportanceRating? ImportanceRating { get; set; }
        }

        public class ApplicationOverviewByGranularity
        {
            public int PendingItems { get; set; }
            public int ApprovedItems { get; set; }
            public int RemediatedItems { get; set; }
            public int FlaggedItems { get; set; }

            public int CompletedItems
            {
                get { return ApprovedItems + RemediatedItems + FlaggedItems; }
            }

            public int TotalItems
            {
                get { return PendingItems + CompletedItems; }
            }

            public int PercentComplete
            {
                get
                {
                    if (TotalItems == 0)
                        return 0;

                    var doneItems = TotalItems - PendingItems;
                    var percentComplete = doneItems / (decimal)TotalItems;

                    return (int)Math.Floor(percentComplete * 100);
                }
            }

            public string PercentCompleteFormatted
            {
                get
                {
                    return PercentComplete + "%";
                }
            }

            public int TitleXOffset
            {
                get
                {
                    if (PercentComplete < 10)
                        return 94;

                    if (PercentComplete == 100)
                        return 75;

                    return 86;
                }
            }
        }

        public override int Size
        {
            get
            {
                return Applications.Count;
            }
        }

        public override byte[] ToExcel()
        {
            var summaryModel = new OverviewSummaryExcelModel(this);

            var excelReport = new ExcelReport
            {
                ReportName = DocumentTitle,
                Renderers = new List<ExcelRendererBase>
                {
                    new ReportHeaderRenderer(DocumentTitle),
                    new SummaryRenderer(this),
                    new TableRenderer("Applications", summaryModel.Applications.OfType<object>().ToList()),
                    new TableRenderer("Segregation Rules", summaryModel.SegregationRules.OfType<object>().ToList()),
                    new ReportFooterRenderer()
                }
            };

            return ToExcel(excelReport);
        }

        public override byte[] ToPDF()
        {
            return ToPDF("Sycorr.PermissionAssist.Reports.Templates.Reviews.Overview.OverviewReport.cshtml");
        }

        public class OverviewSummaryExcelModel
        {
            public class ApplicationSummary
            {
                [TableColumn("Application")]
                public string ApplicationName { get; set; }

                [TableColumn("As Of Date", CellType = CellTypes.Date)]
                public DateTime AsOfDate { get; set; }

                [TableColumn("Importance Rating")]
                public string ImportanceRating { get; set; }

                [TableColumn("Pending Count", CellType = CellTypes.Number)]
                public int? PendingItemCount { get; set; }

                [TableColumn("Approved Count", CellType = CellTypes.Number)]
                public int? ApprovedItemCount { get; set; }

                [TableColumn("Remediated Count", CellType = CellTypes.Number)]
                public int? RemediatedItemCount { get; set; }

                [TableColumn("Flagged Count", CellType = CellTypes.Number)]
                public int? FlaggedItemCount { get; set; }

                [TableColumn("Completed Count", CellType = CellTypes.Number)]
                public int? CompletedItemCount { get; set; }

                [TableColumn("Total Count", CellType = CellTypes.Number)]
                public int? TotalItemCount { get; set; }

                [TableColumn("Percent Complete", CellType = CellTypes.Percent)]
                public decimal? PercentComplete { get; set; }
            }

            public class SegregationRuleSummary
            {
                [TableColumn("Rule")]
                public string Rule { get; set; }

                [TableColumn(IsEmpty = true)]
                public string Dummy1 { get; set; }

                [TableColumn(IsEmpty = true)]
                public string Dummy2 { get; set; }

                [TableColumn(IsEmpty = true)]
                public string Dummy3 { get; set; }

                [TableColumn(IsEmpty = true)]
                public string Dummy4 { get; set; }

                [TableColumn("Remediated Count", CellType = CellTypes.Number)]
                public int? RemediatedItemCount { get; set; }

                [TableColumn("Flagged Count", CellType = CellTypes.Number)]
                public int? FlaggedItemCount { get; set; }

                [TableColumn("Completed Count", CellType = CellTypes.Number)]
                public int? CompletedItemCount { get; set; }

                [TableColumn("Total Count", CellType = CellTypes.Number)]
                public int? TotalItemCount { get; set; }

                [TableColumn("Percent Complete", CellType = CellTypes.Percent)]
                public decimal? PercentComplete { get; set; }
            }

            public List<ApplicationSummary> Applications { get; set; }
            public List<SegregationRuleSummary> SegregationRules { get; set; }

            public OverviewSummaryExcelModel(OverviewModel model)
            {
                Applications = model.Applications
                    .Where(a => a.ApplicationId.HasValue)
                    .Select(a => new ApplicationSummary
                    {
                        ApplicationName = a.Name,
                        AsOfDate = a.AsOfDate.Value,
                        ImportanceRating = !a.ImportanceRating.HasValue ? "" : EnumExtensions.GetName(a.ImportanceRating.Value),
                        PendingItemCount = a.Total.PendingItems,
                        ApprovedItemCount = a.Total.ApprovedItems,
                        RemediatedItemCount = a.Total.RemediatedItems,
                        FlaggedItemCount = a.Total.FlaggedItems,
                        CompletedItemCount = a.Total.CompletedItems,
                        TotalItemCount = a.Total.TotalItems,
                        PercentComplete = a.Total.CompletedItems / (decimal)a.Total.TotalItems
                    }).ToList();

                SegregationRules = model.Applications
                    .Where(s => s.SegregationRuleId.HasValue)
                    .Select(s => new SegregationRuleSummary
                    {
                        Rule = s.Name,
                        RemediatedItemCount = s.Total.RemediatedItems,
                        FlaggedItemCount = s.Total.FlaggedItems,
                        CompletedItemCount = s.Total.CompletedItems,
                        TotalItemCount = s.Total.TotalItems,
                        PercentComplete = s.Total.CompletedItems / (decimal)s.Total.TotalItems
                    }).ToList();
            }
        }

        public class SummaryRenderer : ExcelRendererBase
        {
            private OverviewModel Model { get; set; }

            public SummaryRenderer(OverviewModel model)
            {
                Model = model;
            }

            protected override void RenderInternal()
            {
                var approvalTypes = new[]
                {
                new { Name = "Security Team", IsRequired = Model.Review.AreSecurityTeamReviewsRequired },
                new { Name = "Supervisor", IsRequired = Model.Review.AreSupervisorReviewsRequired },
                new { Name = "Application Owner", IsRequired = Model.Review.AreApplicationOwnerReviewsRequired },
            };

                var dataPoints = new[]
                {
                new { Name = "Review Name", Value = Model.Review.Name },
                new { Name = "Opened Date", Value = Model.Review.OpenedDate.Value.ToString( "MM/dd/yyyy") },
                new { Name = "Closed Date", Value = !Model.Review.ClosedDate.HasValue ? "" : Model.Review.ClosedDate.Value.ToString( "MM/dd/yyyy") },
                new { Name = "Review Type", Value = Model.Review.Type.GetName() },
                new { Name = "Workflow Type", Value = String.Format("{0} Approval ({1})", Model.HowManyPass.Capitalize(), String.Join(", ", approvalTypes.Where(t => t.IsRequired).Select(t=>t.Name))) },
                new { Name = "Applications", Value = Model.Applications.Count(a => a.ApplicationId.HasValue ).ToString() },
                new { Name = "Segregation Rules", Value = Model.Applications.Count(a => a.SegregationRuleId.HasValue ).ToString() },
                new { Name = "Identities", Value = Model.TotalIdentities.ToString() },
                new { Name = "Review Items", Value = Model.Total.TotalItems.ToString() },
                new { Name = "Automatic Approvals", Value = Model.TotalAutoApproved.ToString() },
                new { Name = "Entitlement Roles", Value = Model.TotalRoles.ToString() },
            };

                InsertBlankLine();

                var startRow = Row;

                foreach (var dataPoint in dataPoints)
                {
                    InsertBlankLine();

                    Sheet.Cells[Row, 1].Value = String.Format("  •  {0}:  {1}", dataPoint.Name, dataPoint.Value);
                    Sheet.Cells[Row, 1, Row, 5].Merge = true;
                }

                var row = startRow + 1;

                Sheet.Cells[row, 8].Value = "Status Breakdown";
                Sheet.Cells[row, 8].Style.Font.Size = 12;
                Sheet.Cells[row, 8].Style.Font.Bold = true;
                Sheet.Cells[row, 8, row, 10].Merge = true;

                row++;
                Sheet.Cells[row, 8].Value = "Status";
                Sheet.Cells[row, 9].Value = "Total";
                Sheet.Cells[row, 10].Value = "Percent";
                Sheet.Cells[row, 8, row, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                Sheet.Cells[row, 8, row, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                var statuses = new[]
                {
                    new { Status = "Approved", Count = Model.Total.ApprovedItems, Percent = Model.Total.ApprovedItems / (decimal)Model.Total.TotalItems },
                    new { Status = "Remediated", Count = Model.Total.RemediatedItems, Percent = Model.Total.RemediatedItems / (decimal)Model.Total.TotalItems },
                    new { Status = "Flagged", Count = Model.Total.FlaggedItems, Percent = Model.Total.FlaggedItems / (decimal)Model.Total.TotalItems },
                    new { Status = "Pending", Count = Model.Total.PendingItems, Percent = Model.Total.PendingItems / (decimal)Model.Total.TotalItems },
                };

                foreach (var status in statuses)
                {
                    row++;

                    Sheet.Cells[row, 8].Value = status.Status;
                    Sheet.Cells[row, 9].Value = status.Count;

                    Sheet.Cells[row, 10].Value = status.Percent;
                    Sheet.Cells[row, 10].Style.Numberformat.Format = "0.00%";
                }

                row++;

                row++;
                Sheet.Cells[row, 8].Value = String.Format("  •  Total Completed:  {0}", Model.Total.CompletedItems);
                Sheet.Cells[row, 8, row, 10].Merge = true;

                row++;
                Sheet.Cells[row, 8].Value = String.Format("  •  Percent Completed:  {0:0.00}%", Model.Total.CompletedItems / (decimal)Model.Total.TotalItems * 100);
                Sheet.Cells[row, 8, row, 10].Merge = true;

                row++;
                Sheet.Cells[row, 8, row, 10].Merge = true;
            }
        }

    }
}
