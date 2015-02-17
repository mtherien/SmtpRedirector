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
            var testCommand = SmtpCommand.Parse("MAIL <test@test.com>").Result;
            var socketMock = new Moq.Mock<ISmtpSocketClient>();

            // Act
            try
            {
                testHandler.StartMailRequest(testCommand.Arguments, socketMock.Object);
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

        [TestMethod]
        public void StartMailRequest_BadEmail_ThrowsSmtpErrorRecipientRejected()
        {
            // Arrange
            var testHandler = new MailHandler();
            var testCommand = SmtpCommand.Parse("MAIL FROM:<jmb>").Result;
            var socketMock = new Moq.Mock<ISmtpSocketClient>();

            try
            {
                // Act
                testHandler.StartMailRequest(testCommand.Arguments, socketMock.Object);
            }
            catch (SmtpErrorException smtpErrorException)
            {
                // Assert
                Assert.AreEqual(ResponseCodes.SmtpResponseCode.RecipientRejected, 
                    smtpErrorException.ResponseCode);
                return;
            }

            Assert.Fail("Exception not thrown");
        }

    }
}
