using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Finastra.Hackathon.Emails
{
    public class MailMessageFactory
    {
        public static async Task Send(IEmailModel model)
        {
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

            var templateId = string.Format("GeneratedEmail_{0}", model.View);
            var emailTemplate = getManifestResource(model.View);

            Platform.RazorEngine.AddTemplate(templateId, emailTemplate);

            var body = Platform.RazorEngine.Run(templateId, model);

            MailMessage message = new MailMessage();
            message.Subject = model.Subject;
            message.IsBodyHtml = true;
            message.To.Add(model.ToAddress);
            message.From = model.FromAddress;

            var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);

            var resources = FindLinkedResources(body);

            foreach (var resource in resources)
            {
                view.LinkedResources.Add(resource);
            }

            message.AlternateViews.Add(view);

            using (var smtpClient = new SmtpClient("smtp.postmarkapp.com", 2525) { Credentials = new NetworkCredential("cbcdf121-f50b-465e-8886-7c39ee1d9b34", "cbcdf121-f50b-465e-8886-7c39ee1d9b34") })
            {
                await smtpClient.SendMailAsync(message);
            }
        }

        private static IEnumerable<LinkedResource> FindLinkedResources(string body)
        {
            var retVal = new List<LinkedResource>();
            var matches = Regex.Matches(body, "\"cid:(.*?)\"");

            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Value.StartsWith("\"cid:"))
                        continue;

                    var cid = group.Value;
                    var key = String.Format("Finastra.Hackathon.Emails.Attachments.{0}", cid);
                    var stream = typeof(MailMessageFactory).Assembly.GetManifestResourceStream(key);

                    var resource = new LinkedResource(stream, "image/png")
                    {
                        ContentId = cid,
                        TransferEncoding = TransferEncoding.Base64,
                        ContentType =
                        {
                            MediaType = "image/png",
                            Name = cid
                        },
                        ContentLink = new Uri("cid:" + cid)
                    };

                    retVal.Add(resource);
                }
            }

            return retVal;
        }
    }
}