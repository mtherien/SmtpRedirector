using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmtpRedirector.Server.Smtp;

namespace SmtpRedirector.Server.Tests.Smtp
{
    [TestClass]
    public class SmtpCommandTests
    {
        [TestMethod]
        public void Parse_EHLOWithDomain_ReturnsResult()
        {
            // Arrange
            var testCommand = "EHLO SP test.com";

            // Act
            var result = SmtpCommand.Parse(testCommand);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(SmtpVerb.ExtendedHello, result.Result.Verb);

            var spArgument = result.Result.Arguments.FirstOrDefault(m => m.Argument.Equals(SmtpArgumentName.Sp));
            Assert.IsNotNull(spArgument);
            Assert.AreEqual("test.com", spArgument.Value);
        }

        [TestMethod]
        public void Parse_EHLOWithoutDomain_ReturnsTrueWithNoArguments()
        {
            // Arrange
            var testCommand = "EHLO SP";

            // Act
            var result = SmtpCommand.Parse(testCommand);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.Result.Arguments.Any());
        }

        [TestMethod]
        public void ParseArguments_SPArgument_ReturnsArgumentPair()
        {
            // Arrange
            var testCommandParts =  new string[]
            {
                "TEST","SP","test.com"
            };

            // Act
            var result = SmtpCommand.ParseArguments(testCommandParts);

            // Assert
            var spArgument = result.FirstOrDefault(m => m.Argument.Equals(SmtpArgumentName.Sp));
            Assert.IsNotNull(spArgument,"Did not return an SP argument");
            Assert.AreEqual("test.com",spArgument.Value);
        }

        [TestMethod]
        public void ParseArguments_FromArgument_ReturnsArgumentPair()
        {
            // Arrange
            var testCommandParts = new string[]
            {
                "TEST", "FROM:mm@mm.com"
            };

            // Act
            var result = SmtpCommand.ParseArguments(testCommandParts);

            // Assert
            var spArgument = result.FirstOrDefault(m => m.Argument.Equals(SmtpArgumentName.From));
            Assert.IsNotNull(spArgument, "Did not return a From argument");
            Assert.AreEqual("mm@mm.com", spArgument.Value);
        }

        [TestMethod]
        public void GetVerb_SupportedCommands_ReturnProperVerb()
        {
            // Arrange
            var testCommands = new KeyValuePair<string,SmtpVerb>[]
            {
                new KeyValuePair<string, SmtpVerb>("HELO",SmtpVerb.Hello), 
                new KeyValuePair<string, SmtpVerb>("EHLO",SmtpVerb.ExtendedHello), 
                new KeyValuePair<string, SmtpVerb>("MAIL",SmtpVerb.Mail), 
                new KeyValuePair<string, SmtpVerb>("RCPT",SmtpVerb.Recipient), 
                new KeyValuePair<string, SmtpVerb>("DATA",SmtpVerb.Data), 
                new KeyValuePair<string, SmtpVerb>("RSET",SmtpVerb.Reset), 
                new KeyValuePair<string, SmtpVerb>("VRFY",SmtpVerb.Verify), 
                new KeyValuePair<string, SmtpVerb>("EXPN",SmtpVerb.Expand), 
                new KeyValuePair<string, SmtpVerb>("HELP",SmtpVerb.Help), 
                new KeyValuePair<string, SmtpVerb>("NOOP",SmtpVerb.NoOp), 
                new KeyValuePair<string, SmtpVerb>("QUIT",SmtpVerb.Quit), 
            };

            foreach (var commandPair in testCommands)
            {
                // Act
                var resultingVerb = SmtpCommand.GetVerb(commandPair.Key);

                // Assert
                Assert.AreEqual(commandPair.Value, resultingVerb, 
                    string.Format("Command string {0} did not return verb {1}", 
                        commandPair.Key, commandPair.Value));
            }

        }

    }
}
