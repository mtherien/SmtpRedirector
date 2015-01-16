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
using SmtpRedirector.Server;
using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SmtpListener(new SmtpConfiguration(), new ConsoleLogger(), new MailHandler());
            server.Start();

            System.Console.WriteLine("SMTP Server running.  Enter exit to stop");

            while (true)
            {
                var command = System.Console.ReadLine();
                if (command.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else
                {
                    System.Console.WriteLine("!! Invalid command");
                }
            }

            server.Stop();

        }
    }
}
