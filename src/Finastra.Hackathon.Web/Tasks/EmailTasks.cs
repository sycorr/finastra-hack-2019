using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Finastra.Hackathon.Emails;
using Finastra.Hackathon.Emails.Templates.HelloWorld;

namespace Finastra.Hackathon.Web.Tasks
{
    public class EmailTasks
    {
        public async Task SendAlert(string email)
        {
            //await SendEmail(new MailAddress(email, "Anna Smith (Commercial Lending)"), new MailAddress("insights@sycorr.com", "Insights Alert"), 
            //    "Predictive alert about your client", "Your clients....");

            var model = new HelloWorldEmailModel()
            {
                FromAddress = new MailAddress("insights@sycorr.com", "Insights Alert"),
                ToAddress = new MailAddress(email, "Anna Smith (Commercial Lending)"),
                Name = "Anna"
            };

            await MailMessageFactory.Send(model);

        }

        public async Task SendMessage(string email)
        {
            //await SendEmail(new MailAddress(email, "Insights Alert"), new MailAddress("anna-smith@sycorr.com", "Anna Smith (Commercial Lending)"),
            //    "Predictive alert about your client", "Your clients....");

            var model = new HelloWorldEmailModel()
            {
                FromAddress = new MailAddress("insights@sycorr.com", "Insights Alert"),
                ToAddress = new MailAddress(email, "Anna Smith (Commercial Lending)"),
                Name = "Anna"
            };

            await MailMessageFactory.Send(model);
        }
    }
}
