using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Server.Interfaces
{
    public interface ISmtpSocketClient : ISocketClient
    {
        SmtpCommand LastCommand { get; }
    }
}