using System;
using System.Text.RegularExpressions;
using Autofac;

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
            var myRegex = new Regex(@"^<(.*)> ?(.*)?", RegexOptions.None);

            var groups = myRegex.Match(emailString).Groups;

            if (!groups[1].Success)
            {
                throw new ArgumentException("Email address was an invalid format");
            }

            _email = groups[1].Value;

            _displayName = string.Empty;

            if (groups[2].Success)
            {
                _displayName = groups[2].Value;
            }

        }

        public string Email { get {  return _email; }  }
        public string DisplayName { get { return _displayName; } }
    }
}