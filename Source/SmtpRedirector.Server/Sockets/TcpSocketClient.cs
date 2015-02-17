// Copyright 2015 Mike Therien
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SmtpRedirector.Server.Interfaces;

namespace SmtpRedirector.Server.Sockets
{
    public class TcpSocketClient : ISocketClient
    {
        private readonly TcpClient _tcpClient;

        public TcpSocketClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            EndPoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;

            if (EndPoint != null)
            {
                var hostEntry = Dns.GetHostEntry(EndPoint.Address);
                HostName = hostEntry.HostName;
            }
            else
            {
                HostName = "unknown";
            }
        }

        public string Read()
        {
            var messageBytes = new byte[8192];
            var bytesRead = 0;
            var clientStream = _tcpClient.GetStream();
            var encoder = new ASCIIEncoding();
            bytesRead = clientStream.Read(messageBytes, 0, 8192);
            var strMessage = encoder.GetString(messageBytes, 0, bytesRead);
            return strMessage;
        }

        public string Read(string terminator)
        {
            var readString = new StringBuilder();
            while (!readString.ToString().EndsWith(terminator))
            {
                var newData = Read();
                readString.Append(newData);
            }

            return readString.ToString();
        }

        public void Write(string strMessage)
        {
            var clientStream = _tcpClient.GetStream();
            var encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(strMessage + "\r\n");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        public void Close()
        {
            _tcpClient.Close();
        }

        public IPEndPoint EndPoint { get; private set; }

        public string HostName { get; private set; }
    }
}
