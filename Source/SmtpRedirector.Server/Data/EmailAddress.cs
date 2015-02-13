using System;

namespace SmtpRedirector.Server.Data
{
    public class EmailAddress
    {
        private string _email;
        private string _displayName;

        public EmailAddress(string emailString)
        {
            ParseString(emailString);
        }

        private void ParseString(string emailString)
        {
            if (emailString.Trim().StartsWith("<") && emailString.Trim().EndsWith(">"))
            {
                _email = emailString.Trim().TrimStart('<').TrimEnd('>');
                _displayName = string.Empty;
                return;
            }
            
            throw new ArgumentException("Email address was an invalid format");
        }

        public string Email { get {  return _email; }  }
        public string DisplayName { get { return _displayName; } }
    }
}