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
using SmtpRedirector.Server.Interfaces;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpClientSession : ISmtpClientSession
    {
        private ISmtpConfiguration _configuration;
        private ISmtpSocketClient _client;
        private readonly ILogger _logger;
        private readonly IMailHandler _mailHandler;
        private Guid _sessionId;
        private bool _helloGiven = false;

        public SmtpClientSession(ILogger logger, IMailHandler mailHandler)
        {
            _logger = logger;
            _mailHandler = mailHandler;
        }


        public Guid Init(ISmtpSocketClient client, ISmtpConfiguration configuration)
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
                    strMessage = _client.Read("\r\n");
                }
                catch (Exception e)
                {
                    //a socket error has occured
                    break;
                }

                if (strMessage.Length <=0) continue;

                if (_client.LastCommand == null) continue;


                switch (_client.LastCommand.Verb)
                {
                    case SmtpVerb.Quit:
                        _logger.Info("{0} - Connection from {1} terminated by client", _sessionId, _client.EndPoint);
                        _client.Close();
                        break;
                    case SmtpVerb.ExtendedHello:
                        HandleExtendedHello(_client.LastCommand.Arguments);
                        break;
                    case SmtpVerb.Hello:
                        HandleHello(_client.LastCommand.Arguments);
                        break;
                    case SmtpVerb.Mail:
                        HandleMail(_client.LastCommand.Arguments);
                        break;
                    case SmtpVerb.Reset:
                        HandleReset();
                        break;
                    case SmtpVerb.NoOp:
                        _client.Write("250 OK");
                        break;
                    case SmtpVerb.Verify:
                        _client.Write("500 Not implemented");
                        break;
                    case SmtpVerb.Help:
                        HandleHelp();
                        break;
                }
            }
        }

        private void HandleHelp()
        {
            _client.Write("250 OK");
            _client.ClearLastCommand();
        }

        private void HandleMail(IEnumerable<SmtpArgument> commandArguments)
        {
            if (!_helloGiven)
            {
                _client.Write("503 HELO/EHLO Command not issued");
                return;
            }
            _mailHandler.GetMailMessage(commandArguments.ToArray(), _client);
            _client.ClearLastCommand();
        }

        private void HandleReset()
        {
            _client.Write("250 OK");
            _client.ClearLastCommand();
        }

        private void HandleHello(IEnumerable<SmtpArgument> commandArguments)
        {
            var hostName = commandArguments.FirstOrDefault(m => m.Argument == SmtpArgumentName.Sp);
            _client.Write(string.Format("250 Hello {0} ([{1}]), nice to meet you.", 
                hostName.Value, _client.EndPoint.Address));
            _helloGiven = true;
            _client.ClearLastCommand();
        }

        private void HandleExtendedHello(IEnumerable<SmtpArgument> commandArguments)
        {
            var hostName = commandArguments.FirstOrDefault(m => m.Argument == SmtpArgumentName.Sp);
            _helloGiven = true;
            _client.Write(string.Format("250 Hello {0} ([{1}]), nice to meet you.", 
                hostName.Value, _client.EndPoint.Address));
            _client.Write("250-VRFY");
            _client.Write("250-HELP");
            _client.Write("250-MAIL");
            _client.Write("250-RSET");
            _client.Write("250 NOOP");
            _client.ClearLastCommand();
        }
    }
}
