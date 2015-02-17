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

        private static SmtpVerb GetVerb(string commandVerb)
        {
            switch (commandVerb.Trim().ToUpper())
            {
                case "EHLO":
                    return SmtpVerb.ExtendedHello;
                case "HELO":
                    return SmtpVerb.Hello;
            }

            return SmtpVerb.Unsupported;
        }

        internal static IEnumerable<SmtpArgument> ParseArguments(string[] commandParts)
        {
            for (int i = 1; i < commandParts.Length; i++)
            {
                SmtpArgumentName argumentName;
                switch (commandParts[i].Trim().ToUpper())
                {
                    case "SP":
                        argumentName= SmtpArgumentName.Sp;
                        break;
                    default:
                        argumentName = SmtpArgumentName.None;
                        break;
                }

                var argumentValueIndex = i + 1;
                string argumentValue = null;

                if (argumentValueIndex == commandParts.Length)
                {
                    // No value
                    break;
                }

                argumentValue = commandParts[argumentValueIndex];
                i = argumentValueIndex;

                yield return new SmtpArgument(argumentName, argumentValue);

            }
        }
    }
}
