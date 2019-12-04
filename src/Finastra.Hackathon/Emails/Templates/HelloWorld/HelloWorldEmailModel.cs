using System.Net.Mail;

namespace Finastra.Hackathon.Emails.Templates.HelloWorld
{
    public class HelloWorldEmailModel : IEmailModel
    {
        public HelloWorldEmailModel()
        {
            View = "Finastra.Hackathon.Emails.Templates.HelloWorld.HelloWorld.cshtml";
            Subject = "Hello World";
            Title = "Hello World";
        }

        public string Name { get; set; }

        public string View { get; private set; }
        public string Title { get; private set; }
        public string Subject { get; private set; }
        public MailAddress ToAddress { get; set; }
        public MailAddress FromAddress { get; set; }
    }
}
