namespace SmtpRedirector.Server.Smtp
{
    public class ParseResult<T>
    {
        private readonly bool _success;
        private readonly T _result;

        public ParseResult()
        {
            _success = false;
        } 

        public ParseResult(T result)
        {
            _success = true;
            _result = result;
        }

        public bool Success
        {
            get { return _success; }
        }

        public T Result
        {
            get { return _result; }
        }
    }
}