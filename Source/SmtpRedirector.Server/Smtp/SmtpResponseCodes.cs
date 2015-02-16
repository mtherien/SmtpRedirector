using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpRedirector.Server.Smtp
{
    public class ResponseCodes
    {
        public enum SmtpResponseCode
        {
            SystemStatusOrHelpReply = 211,
            HelpMessage = 214,
            ServiceReady = 220,
            ClosingTransmissionChannel = 221,
            OK = 250,
            UserNotLocalWillForwardTo = 251,
            StartMailInputEndWithDot = 354,
            AuthenticationContinue = 334,
            AuthenticationOK = 235,

            SyntaxErrorCommandUnrecognised = 500,
            SyntaxErrorInCommandArguments = 501,
            CommandNotImplemented = 502,
            BadSequenceOfCommands = 503,
            CommandParameterNotImplemented = 504,
            ExceededStorageAllocation = 552,
            AuthenticationFailure = 535,
            AuthenticationRequired = 530,
            RecipientRejected = 550,
            TransactionFailed = 554,

        }
    }
}
