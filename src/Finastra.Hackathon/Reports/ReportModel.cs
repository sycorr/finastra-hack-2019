using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Finastra.Hackathon.Reports.Excel;

namespace Finastra.Hackathon.Reports
{
    public interface IReportModel
    {
        string DocumentTitle { get; set; }
        string FileName { get; set; }
        IDictionary<string, string> Filters { get; }
        int Size { get; }
        int SizeLimit { get; }

        byte[] ToExcel();
        byte[] ToExcel(ExcelReport excelReport);
        byte[] ToPDF();
        byte[] ToPDF(string view);
        byte[] ToReport(ReportFormat format);

        IReportModel[] PerformanceSplit();
    }

    public abstract class ReportModel : IReportModel
    {
        protected ReportModel()
        {
            Filters = new Dictionary<string, string>();
        }

        public string DocumentTitle { get; set; }
        public string FileName { get; set; }
        public IDictionary<string, string> Filters { get; private set; }

        public abstract int Size { get; }
        public int SizeLimit {
            get { return 10000; }
        }

        public abstract byte[] ToExcel();

        public byte[] ToExcel(ExcelReport excelReport)
        {
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add(excelReport.ReportName);
                var row = 0;
                var columnWidth = 10;

                if (Size > SizeLimit)
                {
                    var warning =
                        String.Format(
                            "The report you were trying to generate exceeded the {0:N0} item limit by {1:N0}.",
                            SizeLimit, (Size - SizeLimit));

                    excelReport = new ExcelReport
                    {
                        ReportName = DocumentTitle,
                        Renderers = new List<ExcelRendererBase>
                        {
                            new ReportHeaderRenderer(DocumentTitle),
                            new FilterRenderer(Filters),
                            new LineRenderer(""),
                            new LineRenderer("It appears this report was too big...", 14f),
                            new LineRenderer(warning, fontBold: false),
                            new LineRenderer("Please create smaller reports by adding additional filters to your report criteria.", fontBold: false),
                            new ReportFooterRenderer()
                        }
                    };
                }
                else
                {
                    columnWidth = excelReport.CalcColumnWidth();
                }

                foreach (var renderer in excelReport.Renderers)
                {
                    row = renderer.Render(sheet, row, columnWidth);
                }

                sheet.Cells.AutoFitColumns();

                package.Workbook.Properties.Author = "Sycorr";
                package.Workbook.Properties.Title = excelReport.ReportName;
                package.Workbook.Properties.Subject = excelReport.ReportName;

                return package.GetAsByteArray();
            }
        }

        public abstract byte[] ToPDF();

        public byte[] ToPDF(string view)
        {
            var options = new HtmlLoadOptions
            {
                PageInfo =
                {
                    Width = Aspose.Pdf.Generator.PageSize.LetterWidth,
                    Height = Aspose.Pdf.Generator.PageSize.LetterHeight,
                    Margin = {Top = 35, Bottom = 40, Right = 35, Left = 35}
                }
            };

            //Sanitize for SSRF and Local File Inclusion attacks if other 
            //protections did not catch user inputed data that is now rendered
            //html raw in the pdf. https://www.virtuesecurity.com/kb/wkhtmltopdf-file-inclusion-vulnerability-2/

            Func<string, string> sanitize = s => s.Replace("iframe", "[iframe]", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("file:", "[file]:", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("http:", "[http]:", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("tcp:", "[tcp]:", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("ftp:", "[ftp]:", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("behavior:", "[behavior]:", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("@import", "@[import]", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("script", "[script]", StringComparison.OrdinalIgnoreCase)
                                                  .Replace("href", "[href]", StringComparison.OrdinalIgnoreCase);

            Func<string, string> getManifestResource = s =>
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(s))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                };
            };

            Action<Document, string> addReportHeader = (pdf, title) =>
            {
                var stamp = new TextStamp(title)
                {
                    TopMargin = 15,
                    LeftMargin = 25,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Opacity = 0.6f,
                };

                stamp.TextState.FontSize = 8;

                foreach (Page page in pdf.Pages)
                {
                    page.AddStamp(stamp);
                }
            };

            Action<Document> addCompanyNameFooter = pdf =>
            {
                var companyName = AutofacResolver.Current.Resolve<ISettingsProvider>().Get(Keys.Settings.General.CompanyName);

                var offset = TimeZoneInfoExtensions.GetCompanyTimeZone().GetUtcOffset(DateTime.UtcNow.ToCompanyTime());

                var stampText = String.Format("Generated on {0} GMT{1}", DateTime.UtcNow.ToCompanyTime("M/d/yy - h:mm tt"), offset.Hours);

                if (!String.IsNullOrWhiteSpace(companyName))
                    stampText = String.Format("Generated by {0} on {1} GMT{2}", companyName, DateTime.UtcNow.ToCompanyTime("M/d/yy - h:mm tt"), offset.Hours);

                var companyStamp = new TextStamp(stampText)
                {
                    BottomMargin = 15,
                    LeftMargin = 25,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Opacity = 0.6f,
                };

                companyStamp.TextState.FontSize = 8;

                foreach (Page page in pdf.Pages)
                {
                    page.AddStamp(companyStamp);
                }
            };

            Action<Document> addPageNumberFooter = pdf =>
            {
                var pageStamp = new TextStamp("")
                {
                    BottomMargin = 15,
                    RightMargin = 25,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Opacity = 0.6f,
                };

                pageStamp.TextState.FontSize = 8;
                var pageCount = 1;

                foreach (Page page in pdf.Pages)
                {
                    pageStamp.Value = "Page: " + pageCount++;
                    page.AddStamp(pageStamp);
                }
            };

            Action<Document> addDocumentInfo = pdf =>
            {
                var d = new DocumentInfo(pdf)
                {
                    Author = "Sycorr",
                    CreationDate = DateTime.UtcNow.ToCompanyTime(),
                    Title = "Permission Assist® " + DocumentTitle,
                    Subject = "Permission Assist® " + DocumentTitle
                };
            };

            Func<string, object, Document> createDocument = (template, model) =>
            {
                var html = RazorEngine.Run(template, model);
                var sanitizedHtml = sanitize(html);

                var buffer = Encoding.UTF8.GetBytes(sanitizedHtml);
                var instream = new MemoryStream(buffer);

                return new Document(instream, options);
            };

            var docs = new List<Document>();
            var splits = new List<IReportModel>().ToArray();

            if (Size < SizeLimit)
            {
                splits = this.PerformanceSplit();
            }
            else
            {
                view = "Sycorr.PermissionAssist.Reports.Templates.Shared.LimitExceeded.cshtml";
                splits = new IReportModel[] { this };
            }

            var templateId = string.Format("GeneratedPDF_{0}", view);
            var modelTemplate = getManifestResource(view);
            RazorEngine.AddTemplate(templateId, modelTemplate);

            foreach (var split in splits)
            {
                var d = createDocument(templateId, split);
                docs.Add(d);
            }

            using (var doc = new Document())
            {
                foreach (var d in docs)
                {
                    doc.Pages.Add(d.Pages);
                }

                addReportHeader.Invoke(doc, ("Permission Assist® " + DocumentTitle).Trim());
                addCompanyNameFooter.Invoke(doc);
                addPageNumberFooter.Invoke(doc);
                addDocumentInfo.Invoke(doc);

                var privilege = DocumentPrivilege.ForbidAll;
                privilege.ChangeAllowLevel = 1;
                privilege.AllowPrint = true;
                privilege.AllowCopy = true;

                var fileSecurity = new PdfFileSecurity(doc);
                fileSecurity.SetPrivilege(privilege);

                using (var outstream = new MemoryStream())
                {
                    doc.Save(outstream);
                    return outstream.GetBuffer();
                }
            }
        }

        public byte[] ToReport(ReportFormat format)
        {
            if (format == ReportFormat.Excel)
                return ToExcel();

            if (format == ReportFormat.PDF)
                return ToPDF();

            return null;
        }

        public virtual IReportModel[] PerformanceSplit()
        {
            return new IReportModel[] { this };
        }
    }
}