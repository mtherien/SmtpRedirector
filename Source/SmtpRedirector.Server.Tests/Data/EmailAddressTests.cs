using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmtpRedirector.Server.Data;

namespace SmtpRedirector.Server.Tests.Data
{
    [TestClass]
    public class EmailAddressTests
    {
        [TestMethod]
        public void Constructor_EmailAddressInBrackets_ReturnsEmailAddress()
        {
            // Arrange
            var testEmail = "<mytest@test.com>";

            // Act
            var emailAddress = new EmailAddress(testEmail);

            // Assert
            Assert.AreEqual("mytest@test.com", emailAddress.Email);
        }
    }
}
