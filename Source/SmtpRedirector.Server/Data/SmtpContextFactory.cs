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
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SmtpRedirector.Server.Data
{
    public class SmtpContextFactory
    {
        private const string DataFileName = "SmtpRedirector.data";
        public static SmtpContext Get(string dataFolder)
        {
            var file = Path.Combine(dataFolder, DataFileName);

            if (!File.Exists(file))
            {
                SQLiteConnection.CreateFile(file);    
            }
            
            var connection = new SQLiteConnection(string.Format("Data Source=|DataDirectory|{0}", file));
            
            var smtpContext = new SmtpContext(connection);

            return smtpContext;
        }
    }
}
