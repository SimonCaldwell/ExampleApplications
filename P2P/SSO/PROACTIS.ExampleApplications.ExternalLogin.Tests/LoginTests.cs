/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PROACTIS.ExampleApplications.ExternalLogin.Tests
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        [TestCategory("API")]
        public void CheckAUserIsLoggedOnIfTheyProvideTheCorrectCredentials()
        {
            var service = new Services();
            var userName = "example";
            var password = "secret";
            var databaseTitle = "PROACTIS";
            var actualResult = false;

            if (service.UseAsynchronousImplementation)
                actualResult = service.LoginAsync(userName, password, databaseTitle).Result;
            else
                actualResult = service.Login(userName, password, databaseTitle);

            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        [TestCategory("API")]
        public void CheckAUserIsNotLoggedOnIfTheyProvideTheWrongCredentials()
        {
            var service = new Services();
            var userName = "example";
            var password = "rubbish";
            var databaseTitle = "PROACTIS";
            var actualResult = false;

            if (service.UseAsynchronousImplementation)
                actualResult = service.LoginAsync(userName, password, databaseTitle).Result;
            else
                actualResult = service.Login(userName, password, databaseTitle);

            Assert.IsFalse(actualResult);
        }
    }
}
