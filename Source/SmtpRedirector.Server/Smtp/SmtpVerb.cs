namespace SmtpRedirector.Server.Smtp
{
    public enum SmtpVerb
    {
        Unsupported,
        /// <summary>
        /// EHLO
        /// </summary>
        ExtendedHello, 
        /// <summary>
        /// HELO
        /// </summary>
        Hello,
        Mail,
        /// <summary>
        /// RCPT
        /// </summary>
        Recipient,
        Data,
        /// <summary>
        /// RSET
        /// </summary>
        Reset,
        /// <summary>
        /// VRFY
        /// </summary>
        Verify,
        /// <summary>
        /// EXPN
        /// </summary>
        Expand,
        Help,
        NoOp,
        Quit

    }
}