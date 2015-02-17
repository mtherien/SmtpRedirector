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
        private readonly ISocketClient _socketClient;

        public SmtpSocketClient(ISocketClient socketClient)
        {
            _socketClient = socketClient;
        }

        public string Read()
        {
            var dataRead = _socketClient.Read();
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
            var dataRead = _socketClient.Read(terminator);
            CheckDataForCommand(dataRead);
            return dataRead;
        }

        public void Write(string data)
        {
            _socketClient.Write(data);
        }

        public void Close()
        {
            _socketClient.Close();
        }

        public IPEndPoint EndPoint
        {
            get { return _socketClient.EndPoint; }
        }

        public string HostName
        {
            get { return _socketClient.HostName; }
        }

        public SmtpCommand LastCommand { private set; get; }
        public void ClearLastCommand()
        {
            LastCommand = null;
        }
    }
}
