using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpErrorException :Exception
    {
        private readonly ResponseCodes.SmtpResponseCode _responseCode;
        private readonly string _messageText;

        public SmtpErrorException(ResponseCodes.SmtpResponseCode responseCode, string messageText, Exception innerException) :
            base(string.Format("{0}:{1}", responseCode,messageText), innerException)
        {
            _responseCode = responseCode;
            _messageText = messageText;
        }

        public SmtpErrorException(ResponseCodes.SmtpResponseCode responseCode, string messageText) :
            this(responseCode, messageText, null)
        {
        }

        public ResponseCodes.SmtpResponseCode ResponseCode
        {
            get { return _responseCode; }
        }

        public string MessageText
        {
            get { return _messageText; }
        }

        public string SMTPResponse
        {
            get { return string.Format("{0:D} {1}", ResponseCode, MessageText); }
        }
    }
}
