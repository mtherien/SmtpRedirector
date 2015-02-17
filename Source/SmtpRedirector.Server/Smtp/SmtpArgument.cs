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
}