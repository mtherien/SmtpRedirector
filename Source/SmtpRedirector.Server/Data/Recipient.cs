using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Server.Data
{
    public class Recipient 
    {
        public RecipientType RecipientType { get; set; }
        public EmailAddress EmailAddress { get; set; }

        public Recipient(string emailString, RecipientType recipientType) 
        {
            RecipientType = recipientType;
            EmailAddress = new EmailAddress(emailString);
        }
    }
}