using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmtpRedirector.Server.Interfaces;
using SmtpRedirector.Server.Smtp;
using SmtpRedirector.Server.Sockets;

namespace SmtpRedirector.Server.Tests.Sockets
{
    [TestClass]
    public class SmtpSocketClientTests
    {
        [TestMethod]
        public void Read_WithCommand_SetsLastCommand()
        {
            // Arrange
            var testCommand = "MAIL FROM:mm@mm.com";
            var socketMock = new Mock<ISocketClient>();
            socketMock.Setup(m => m.Read())
                .Returns(testCommand);

            // Act
            var testClient = new SmtpSocketClient(socketMock.Object);
            testClient.Read();

            // Assert
            Assert.IsNotNull(testClient.LastCommand);
            Assert.AreEqual(SmtpVerb.Mail, testClient.LastCommand.Verb);


        }
    }
}
