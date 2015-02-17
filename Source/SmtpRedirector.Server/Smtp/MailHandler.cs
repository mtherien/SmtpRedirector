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
using System.Text;
using System.Threading.Tasks;
using SmtpRedirector.Server.Data;
using SmtpRedirector.Server.Interfaces;

namespace SmtpRedirector.Server.Smtp
{
    public class MailHandler : IMailHandler
    {
        public MailMessage GetMailMessage(SmtpArgument[] mailCommandArguments, ISmtpSocketClient client)
        {
            var fromAddress = mailCommandArguments.GetValue(SmtpArgumentName.From);
            if (fromAddress == null)
            {
                throw new SmtpErrorException(ResponseCodes.SmtpResponseCode.SyntaxErrorInCommandArguments, "FROM: not included");
            }

            EmailAddress emailAddress = null;
            try
            {
                emailAddress = new EmailAddress(fromAddress);
            }
            catch (ArgumentException argumentException)
            {
                throw new SmtpErrorException(ResponseCodes.SmtpResponseCode.RecipientRejected, "Address is invalid", argumentException);
            }

            return ProcessMailCommands(client,new MailMessage(emailAddress));
        }

        internal MailMessage ProcessMailCommands(ISmtpSocketClient client, MailMessage mailMessage)
        {
            while (true)
            {
                client.ClearLastCommand();
                var lastData = client.Read("\r\n");

                if (lastData == ".")
                {
                    break;
                }

                if (client.LastCommand == null)
                {
                    continue;
                }
            }

            return mailMessage;
        }

    }


}
