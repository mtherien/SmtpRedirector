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

using System.Threading;

namespace SmtpRedirector.Server.Smtp
{
    internal class SmtpProcessInfo
    {
        private Thread _processThread = null;
        private readonly ISmtpClientSession _clientSession;

        public SmtpProcessInfo(ISmtpClientSession clientSession)
        {
            _clientSession = clientSession;
        }

        public bool IsAlive
        {
            get { return _processThread != null && _processThread.IsAlive; }
        }

        public void Stop()
        {
            if (_processThread.IsAlive)
            {
                _processThread.Abort();
            }
        }

        public void Start()
        {
            _processThread = new Thread(new ThreadStart(_clientSession.HandleSession));
            _processThread.Start();
        }
    }
}