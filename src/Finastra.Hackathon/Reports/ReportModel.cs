﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Aspose.Pdf;
using Aspose.Pdf.Facades;

namespace Finastra.Hackathon.Reports
{
    public interface IReportModel
    {
        string DocumentTitle { get; set; }
        string FileName { get; set; }
        string View { get; set; }
        byte[] ToPDF();
    }

    public abstract class ReportModel : IReportModel
    {
        public string DocumentTitle { get; set; }
        public string FileName { get; set; }
        public string View { get; set; }

        public byte[] ToPDF()
        {
            var options = new HtmlLoadOptions
            {
                PageInfo =
                {
                    Width = PageSize.A4.Width,
                    Height = PageSize.A4.Height,
                    Margin = {Top = 35, Bottom = 40, Right = 35, Left = 35}
                }
            };

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
                var stampText = String.Format("Generated on {0}", DateTime.Now.ToString("M/d/yy - h:mm tt"));

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
                    Author = "First Bank of Valkyrie",
                    CreationDate = DateTime.Now,
                    Title = "First Bank of Valkyrie " + DocumentTitle,
                    Subject = "First Bank of Valkyrie " + DocumentTitle
                };
            };

            Func<string, object, Document> createDocument = (template, model) =>
            {
                var html = Platform.RazorEngine.Run(template, model);

                var buffer = Encoding.UTF8.GetBytes(html);
                var instream = new MemoryStream(buffer);

                return new Document(instream, options);
            };

            var docs = new List<Document>();
           
            var templateId = string.Format("GeneratedPDF_{0}", View);
            var modelTemplate = getManifestResource(View);
            Platform.RazorEngine.AddTemplate(templateId, modelTemplate);

            docs.Add(createDocument(templateId, this));

            using (var doc = new Document())
            {
                foreach (var d in docs)
                {
                    doc.Pages.Add(d.Pages);
                }

                addReportHeader.Invoke(doc, ("First Bank of Valkyrie " + DocumentTitle).Trim());
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
    }
}