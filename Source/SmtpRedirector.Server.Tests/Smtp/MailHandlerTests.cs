using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmtpRedirector.Server.Interfaces;
using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Server.Tests.Smtp
{
    [TestClass]
    public class MailHandlerTests
    {
        [TestMethod]
        public void StartMailRequest_MissingFrom_ThrowsSmtpErrorException501()
        {
            // Arrange
            var testHandler = new MailHandler();
            var testCommand = "<test@test.com>";
            var socketMock = new Moq.Mock<ISocketClient>();

            // Act
            try
            {
                testHandler.StartMailRequest(testCommand, socketMock.Object);
            }
            catch (SmtpErrorException smtpErrorException)
            {
                // Assert
                Assert.AreEqual(ResponseCodes.SmtpResponseCode.SyntaxErrorInCommandArguments, 
                    smtpErrorException.ResponseCode);
                return;
            }

            Assert.Fail("Exception not thrown");
            
        }

        public void StartMailRequest_BadEmail_ThrowsSmtpErrorException()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
