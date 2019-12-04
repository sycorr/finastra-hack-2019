using System;
using System.Collections.Generic;
using System.Linq;
using Finastra.Hackathon.Reports.Excel;

namespace Finastra.Hackathon.Reports.Templates.Identities.List
{
    public class IdentitiesListReportModel : ReportModel
    {
        public IdentitiesListReportModel()
        {
            DocumentTitle = "Identities";
            FileName = String.Format("Permission Assist - Identities - {0}", DateTime.UtcNow.ToCompanyTime("dd_MMM_yyyy"));
        }

        public IEnumerable<IdentitySummary> IdentitySummaries { get; set; }

        public override int Size
        {
            get
            {
                return IdentitySummaries.Count();
            }
        }

        public override byte[] ToExcel()
        {
            var excelSource = IdentitySummaries.Select(s => (object)new IdentitiesExcelModel(s)).ToList();

            var excelReport = new ExcelReport
            {
                ReportName = DocumentTitle,
                Renderers = new List<ExcelRendererBase>
                    {
                        new ReportHeaderRenderer(DocumentTitle),
                        new FilterRenderer(Filters),
                        new TableRenderer(DocumentTitle, excelSource),
                        new ReportFooterRenderer()
                    }
            };

            return ToExcel(excelReport);
        }

        public override byte[] ToPDF()
        {
            return ToPDF("Sycorr.PermissionAssist.Reports.Templates.Identities.List.IdentitiesListReport.cshtml");
        }

        private class IdentitiesExcelModel
        {
            [TableColumn("First Name")]
            public string FirstName { get; set; }

            [TableColumn("Last Name")]
            public string LastName { get; set; }

            [TableColumn("Username")]
            public string Username { get; set; }

            [TableColumn("Title")]
            public string Title { get; set; }

            [TableColumn("Supervisor")]
            public string SupervisorName { get; set; }

            [TableColumn("Email")]
            public string EmailAddress { get; set; }

            [TableColumn("Directory ID")]
            public string DirectoryId { get; set; }

            [TableColumn("Directory Lookup")]
            public string DirectoryUsername { get; set; }

            [TableColumn("Employee ID")]
            public string EmployeeId { get; set; }

            [TableColumn("Status")]
            public string Status { get; set; }

            [TableColumn("Type")]
            public string Type { get; set; }

            [TableColumn("First Seen", CellType = CellTypes.Date)]
            public DateTime CreatedDate { get; set; }

            public IdentitiesExcelModel(IdentitySummary identity)
            {
                FirstName = identity.FirstName;
                LastName = identity.LastName;
                Username = identity.Username;
                Title = identity.Title;
                SupervisorName = identity.SupervisorName;
                EmailAddress = identity.EmailAddress;
                DirectoryId = identity.DirectoryId;
                DirectoryUsername = identity.DirectoryUsername;
                EmployeeId = identity.EmployeeId;
                Status = identity.IsActive ? "Active" : "DISABLED";
                Type = identity.Type.GetDisplayName();
                CreatedDate = identity.CreatedDate.GetValueOrDefault();
            }
        }
    }
}