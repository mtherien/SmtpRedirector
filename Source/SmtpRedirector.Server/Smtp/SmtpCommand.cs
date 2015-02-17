using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpCommand
    {
        private readonly SmtpVerb _verb;
        private readonly List<SmtpArgument> _arguments;

        public SmtpCommand(SmtpVerb verb, params SmtpArgument[] arguments)
        {
            _verb = verb;
            _arguments = new List<SmtpArgument>(arguments);
        }

        public SmtpVerb Verb
        {
            get { return _verb; }
        }

        public IEnumerable<SmtpArgument> Arguments
        {
            get { return _arguments; }
        }

        public static ParseResult<SmtpCommand> Parse(string dataReceived)
        {
            var commandParts = dataReceived.Split(' ');
            var verb = GetVerb(commandParts[0]);

            if (verb == SmtpVerb.Unsupported)
            {
                return new ParseResult<SmtpCommand>();
            }

            var arguments = ParseArguments(commandParts).ToArray();
            var smtpCommand = new SmtpCommand(verb, arguments);

            return new ParseResult<SmtpCommand>(smtpCommand);
        }

        public static SmtpVerb GetVerb(string commandString)
        {
            switch (commandString.Trim().ToUpper())
            {
                case "EHLO":
                    return SmtpVerb.ExtendedHello;
                case "HELO":
                    return SmtpVerb.Hello;
                case "MAIL":
                    return SmtpVerb.Mail;
                case "RCPT":
                    return SmtpVerb.Recipient;
                case "DATA":
                    return SmtpVerb.Data;
                case "RSET":
                    return SmtpVerb.Reset;
                case "VRFY":
                    return SmtpVerb.Verify;
                case "EXPN":
                    return SmtpVerb.Expand;
                case "HELP":
                    return SmtpVerb.Help;
                case "NOOP":
                    return SmtpVerb.NoOp;
                case "QUIT":
                    return SmtpVerb.Quit;
            }

            return SmtpVerb.Unsupported;
        }

        internal static IEnumerable<SmtpArgument> ParseArguments(string[] commandParts)
        {
            for (int i = 1; i < commandParts.Length; i++)
            {
                var argumentString = commandParts[i];
                string argumentValue = null;

                if (argumentString.Contains(':'))
                {
                    var argumentStringPair = argumentString.Split(new char[] {':'}, 2);
                    argumentString = argumentStringPair[0];
                    argumentValue = argumentStringPair[1];
                }

                SmtpArgumentName argumentName;
                switch (argumentString.Trim().ToUpper())
                {
                    case "SP":
                        argumentName= SmtpArgumentName.Sp;
                        break;
                    case "FROM":
                        argumentName = SmtpArgumentName.From;
                        break;
                    default:
                        argumentName = SmtpArgumentName.None;
                        break;
                }

                if (argumentValue == null)
                {
                    var argumentValueIndex = i + 1;

                    if (argumentValueIndex == commandParts.Length)
                    {
                        // No value
                        break;
                    }

                    argumentValue = commandParts[argumentValueIndex];
                    i = argumentValueIndex;
                }

                yield return new SmtpArgument(argumentName, argumentValue);
            }
        }
    }
}
