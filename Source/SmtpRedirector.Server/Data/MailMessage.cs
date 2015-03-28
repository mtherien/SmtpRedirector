using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmtpRedirector.Server.Data
{
    public class MailMessage
    {
        public Guid Id { get; set; }
        public DateTime RecievedDateTime { get; set; }
        public string SourceHostname { get; set; }
        public IPAddress SourceIpAddress { get; set; }
        public ISet<Recipient> Recipients { get; set; } 
        public EmailAddress FromAddress { get; set; }
        public EmailBody Body { get; set; }
        public ISet<Header> Headers { get; set; } 

        public MailMessage(EmailAddress fromAddress)
        {
            FromAddress = fromAddress;
        }


    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class EmailBody
    {
    }
}
