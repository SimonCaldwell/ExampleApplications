/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
using System;
using System.Threading.Tasks;
using PROACTIS.P2P.grsCustInterfaces;

namespace PROACTIS.ExampleApplications.ExternalLogin
{
    /// <summary>
    /// Implement the ILogin interface in order to provide your own login validation.
    /// </summary>
    public class Services : ILogin
    {

        /// <summary>
        /// Should P2P call the Login or LoginAsync method.   
        /// Return FALSE to call the Login method.
        /// </summary>
        public bool UseAsynchronousImplementation
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Validate that the user's credentials (username+password) are valid on this database.
        /// </summary>
        /// <param name="userName">Login ID entered by the user</param>
        /// <param name="password">Password entered by the user</param>
        /// <param name="databaseTitle">Database title selected by the user</param>
        /// <returns>True to successfully log in a user,  False if the credentials aren't valid</returns>
        public bool Login(string userName, string password, string databaseTitle)
        {
            if (userName.ToLower() == "example" && password == "secret")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validate that the user's credentials (username+password) are valid on this database.
        /// </summary>
        /// <param name="userName">Login ID entered by the user</param>
        /// <param name="password">Password entered by the user</param>
        /// <param name="databaseTitle">Database title selected by the user</param>
        /// <returns>True to successfully log in a user,  False if the credentials aren't valid</returns>
        public Task<bool> LoginAsync(string userName, string password, string databaseTitle)
        {
            throw new NotImplementedException();
        }
    }
}
