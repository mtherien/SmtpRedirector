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
using System.Net;

namespace SmtpRedirector.Server.Interfaces
{
    public interface ILogger
    {
        void Info(string format, params object[] parameters);
        void Info(string message);
        void Error(Exception exception, string messageFormatToLog, params object[] messageArguments);
        void Error(string messageFormat, params object[] messageArguments);
    }
}
