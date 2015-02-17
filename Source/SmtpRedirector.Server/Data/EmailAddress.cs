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

            if (!HasHostName(groups[1].Value))
            {
                throw new ArgumentException("Email address does not have a host name");
            }

            if (!HasDomainSuffix(groups[1].Value))
            {
                throw new ArgumentException("Email address does not have a domain suffix");
            }

            _email = groups[1].Value;

            _displayName = string.Empty;

            if (groups[2].Success)
            {
                _displayName = groups[2].Value;
            }

        }

        private bool HasDomainSuffix(string email)
        {
            if (!HasHostName(email)) return false;

            var emailParts = email.Split('@');
            var hostParts = emailParts[1].Split('.');
            return hostParts.Length >= 2;
        }

        private bool HasHostName(string email)
        {
            var emailParts = email.Split('@');
            return emailParts.Length == 2;
        }

        public string Email { get {  return _email; }  }
        public string DisplayName { get { return _displayName; } }
    }
}