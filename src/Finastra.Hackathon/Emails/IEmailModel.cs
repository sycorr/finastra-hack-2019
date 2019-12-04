using System.Net.Mail;

namespace Finastra.Hackathon.Emails
{
    public interface IEmailModel
    {
        string View { get; }
        string Subject { get; }
        string Title { get; }
        MailAddress ToAddress { get; set; }
        MailAddress FromAddress { get; }
    }
}