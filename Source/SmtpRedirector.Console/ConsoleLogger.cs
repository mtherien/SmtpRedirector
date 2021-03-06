﻿// Copyright 2015 Mike Therien
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
using System.Text;
using System.Threading.Tasks;
using SmtpRedirector.Server.Interfaces;

namespace SmtpRedirector.Console
{
    public class ConsoleLogger : ILogger
    {
        private string _lock = "lock";

        public void Info(string format, params object[] parameters)
        {
            Info(string.Format(format,parameters));
        }

        public void Info(string message)
        {
            lock (_lock)
            {
                System.Console.WriteLine("{0:yy-MM-dd HH:mm:ss.ffff} - {1}", DateTime.Now, message);
            }
        }

        public void Error(Exception exception, string messageFormatToLog, params object[] messageArguments)
        {
            lock (_lock)
            {
                var message = string.Format(messageFormatToLog, messageArguments);
                if (exception == null)
                {
                    System.Console.Error.WriteLine("{0:yy-MM-dd HH:mm:ss.ffff} - {1}",
                        DateTime.Now, message);
                }
                else
                {
                    System.Console.Error.WriteLine("{0:yy-MM-dd HH:mm:ss.ffff} - {1}\n\r{2}",
                        DateTime.Now, message, exception);
                }
            }
        }

        public void Error(string messageFormat, params object[] messageArguments)
        {
            Error(null,messageFormat,messageArguments);
        }
    }
}
