using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpArgument 
    {
        private readonly SmtpArgumentName _argument;
        private readonly string _value;

        public SmtpArgument(SmtpArgumentName argument, string value)
        {
            _argument = argument;
            _value = value;
        }

        public SmtpArgumentName Argument
        {
            get { return _argument; }
        }

        public string Value
        {
            get { return _value; }
        }
    }

    public static class SmtpArgumentExtensions
    {
        public static string GetValue(this IEnumerable<SmtpArgument> arguments, SmtpArgumentName argumentName)
        {
            var argument = arguments.FirstOrDefault(m => m.Argument == argumentName);

            return argument == null ? null : argument.Value;
        }
    }
}