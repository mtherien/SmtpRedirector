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
using SmtpRedirector.Server.Interfaces;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpClientSession : ISmtpClientSession
    {
        private ISmtpConfiguration _configuration;
        private ISocketClient _client;
        private readonly ILogger _logger;
        private readonly IMailHandler _mailHandler;
        private Guid _sessionId;
        private bool _helloGiven = false;

        public SmtpClientSession(ILogger logger, IMailHandler mailHandler)
        {
            _logger = logger;
            _mailHandler = mailHandler;
        }


        public Guid Init(ISocketClient client, ISmtpConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (configuration == null) throw new ArgumentNullException("configuration");

            _client = client;
            _configuration = configuration;
            _sessionId = Guid.NewGuid();

            return _sessionId;
        }

        public void HandleSession()
        {
            if (_client == null ||
                _configuration == null ||
                _logger == null)
            {
                throw new Exception("Call Init before HandleSession");
            }

            _logger.Info("{0} - Connection from {1}", _sessionId, _client.EndPoint);

            _client.Write("220 localhost -- SMTP Redirector Server");
            string strMessage;

            while (true)
            {
                try
                {
                    strMessage = _client.Read();
                }
                catch (Exception e)
                {
                    //a socket error has occured
                    break;
                }

                if (strMessage.Length <=0) continue;

                var commandParts = strMessage.Split(new char[] {' '}, 2);
                var command = commandParts[0];
                var commandParameter = commandParts.Length > 1
                    ? commandParts[1]
                    : string.Empty;


                switch (command)
                {
                    case "QUIT":
                        _logger.Info("{0} - Connection from {1} terminated by client", _sessionId, _client.EndPoint);
                        _client.Close();
                        break;
                    case "EHLO":
                        HandleExtendedHello(commandParameter);
                        break;
                    case "HELO":
                        HandleHello(commandParameter);
                        break;
                    case "MAIL":
                        HandleMail(commandParameter);
                        break;
                    case "RSET":
                        HandleReset();
                        break;
                    case "NOOP":
                        _client.Write("250 OK");
                        break;
                    case "VRFY":
                        _client.Write("500 Not implemented");
                        break;
                    case "HELP":
                        HandleHelp();
                        break;
                }
            }
        }

        private void HandleHelp()
        {
            _client.Write("250 OK");
        }

        private void HandleMail(string commandParameter)
        {
            if (!_helloGiven)
            {
                _client.Write("503 HELO/EHLO Command not issued");
                return;
            }
            _mailHandler.HandleRequest(commandParameter, _client);
        }

        private void HandleReset()
        {
            _client.Write("250 OK");
        }

        private void HandleHello(string commandParameter)
        {
            _client.Write(string.Format("250 Hello {0} ([{1}]), nice to meet you.", commandParameter, _client.EndPoint.Address));
            _helloGiven = true;
        }

        private void HandleExtendedHello(string commandParameter)
        {
            _helloGiven = true;
            _client.Write(string.Format("250 Hello {0} ([{1}]), nice to meet you.", commandParameter, _client.EndPoint.Address));
            _client.Write("250-VRFY");
            _client.Write("250-HELP");
            _client.Write("250-MAIL");
            _client.Write("250-RSET");
            _client.Write("250 NOOP");
        }
    }
}
