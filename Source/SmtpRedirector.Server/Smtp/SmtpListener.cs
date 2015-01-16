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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SmtpRedirector.Server.Interfaces;
using SmtpRedirector.Server.Sockets;

namespace SmtpRedirector.Server.Smtp
{
    public class SmtpListener
    {
        private readonly SmtpConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMailHandler _mailHandler;
        private readonly List<SmtpProcessInfo> _processes = new List<SmtpProcessInfo>();
        private bool _stopProcessing = false;
        private TcpListener _listener;

        public SmtpListener(SmtpConfiguration configuration, ILogger logger, IMailHandler mailHandler)
        {
            _configuration = configuration;
            _logger = logger;
            _mailHandler = mailHandler;
        }

        public async void Start()
        {
            ThreadPool.SetMinThreads(100, 100);

            var endPoint = new IPEndPoint(IPAddress.Any, 25);
            _listener = new TcpListener(endPoint);
            _listener.Start();

            while (!_stopProcessing)
            {
                TcpClient client;
                try
                {
                    client = await _listener.AcceptTcpClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    // Happens when listening stops
                    continue;
                }

                if (_stopProcessing) break;

                var process = new SmtpClientSession(_logger,_mailHandler);
                process.Init(new TcpSocketClient(client), _configuration);
                StartProcess(process);
            }
        }

        private void StartProcess(SmtpClientSession clientSession)
        {
            var processInfo = new SmtpProcessInfo(clientSession);
            _processes.Add(processInfo);

            processInfo.Start();
        }

        public void Stop()
        {
            _stopProcessing = true;
            _listener.Stop();
            foreach (var smtpProcessInfo in _processes)
            {
                smtpProcessInfo.Stop();
            }
        }
    }
}
