using System;
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

    }
}
