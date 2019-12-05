using System.Net.Mail;

namespace Finastra.Hackathon.Emails.Templates.AlertLender
{
    public class AlertLenderEmailModel : IEmailModel
    {
        public AlertLenderEmailModel()
        {
            View = "Finastra.Hackathon.Emails.Templates.AlertLender.AlertLender.cshtml";
            Subject = "💡  Predictive analysis warning";
            Title = "Predictive analysis warning";
        }

        public string Name { get; set; }
        public string CustomerName { get; set; }

        public string View { get; private set; }
        public string Title { get; private set; }
        public string Subject { get; private set; }
        public MailAddress ToAddress { get; set; }
        public MailAddress FromAddress { get; set; }
    }
}
