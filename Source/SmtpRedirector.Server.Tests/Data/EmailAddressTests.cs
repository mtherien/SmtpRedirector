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
            Assert.AreEqual(string.Empty,emailAddress.DisplayName);
        }

        [TestMethod]
        public void Constructor_EmailAddressNoBrackets_ThrowsArgumentException()
        {
            // Arrange
            var testEmail = "mytest@test.com";
            var exceptionThrown = false;

            try
            {
                // Act
                var emailAddress = new EmailAddress(testEmail);
            }
            catch (ArgumentException)
            {
                // Assert
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown,"Exception was not thrown");
        }

        [TestMethod]
        public void Constructor_EmailAddressWithDisplayName_ParsesCorrectly()
        {
            // Arrange
            var testEmail = "<mytest@test.com> First Last";

            // Act
            var emailAddress = new EmailAddress(testEmail);

            // Assert
            Assert.AreEqual("First Last", emailAddress.DisplayName);
            
        }

        [TestMethod]
        public void Constructor_EmailAddressWithoutDomain_ThrowsArgumentException()
        {
            // Arrange
            var testEmail = "<test>";
            var exceptionThrown = false;

            try
            {
                // Act
                var emailAddress = new EmailAddress(testEmail);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown,"Exception not thrown");
        }

        [TestMethod]
        public void Constructor_EmailAddressWithoutDomainSuffix_ThrowsArgumentException()
        {
            // Arrange
            var testEmail = "<test@test>";
            var exceptionThrown = false;

            try
            {
                // Act
                var emailAddress = new EmailAddress(testEmail);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Exception not thrown");
        }
    }
}
