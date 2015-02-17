using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SmtpRedirector.Server.Interfaces;
using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Server.Sockets
{
    public class SmtpSocketClient : ISmtpSocketClient
    {
        private readonly TcpSocketClient _tcpSocketClient;

        public SmtpSocketClient(TcpClient tcpClient )
        {
            _tcpSocketClient = new TcpSocketClient(tcpClient);
        }

        public string Read()
        {
            var dataRead = _tcpSocketClient.Read();
            CheckDataForCommand(dataRead);
            return dataRead;
        }

        private void CheckDataForCommand(string dataRead)
        {
            var parseResult = SmtpCommand.Parse(dataRead);
            if (parseResult.Success)
            {
                LastCommand = parseResult.Result;
            }
        }

        public string Read(string terminator)
        {
            var dataRead = _tcpSocketClient.Read(terminator);
            CheckDataForCommand(dataRead);
            return dataRead;
        }

        public void Write(string data)
        {
            _tcpSocketClient.Write(data);
        }

        public void Close()
        {
            _tcpSocketClient.Close();
        }

        public IPEndPoint EndPoint
        {
            get { return _tcpSocketClient.EndPoint; }
        }

        public SmtpCommand LastCommand { private set; get; }
    }
}
