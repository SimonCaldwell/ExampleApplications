using System;
using System.IO;
using PROACTIS.P2P.grsCustInterfaces;
using System.Net.Mail;

namespace PROACTIS.ExampleApplication.SimpleCommitmentPosting
{
    public class Services : PROACTIS.P2P.grsCustInterfaces.ICommitmentProcessor
    {
        /// <summary>
        /// Simple example where each exported commitment gets written to it's own xml file in the folder c:\temp
        /// </summary>
        /// <param name="commitmentGUID"></param>
        /// <param name="commitmentXML"></param>
        /// <param name="database"></param>
        /// <param name="databaseServer"></param>
        void ICommitmentProcessor.ProcessCommitment(Guid commitmentGUID, string commitmentXML, string database, string databaseServer)
        {
            try
            {
                var filename = Path.Combine(@"c:\temp", commitmentGUID.ToString() + ".xml");
                File.WriteAllText(filename, commitmentXML);
            }
            catch (Exception e)
            {
                SendErrorEmail(e.Message, commitmentGUID);
                throw;
            }

        }

        private void SendErrorEmail(string message, Guid commitmentGUID)
        {
            var mail = new MailMessage("errors@proactis.com", "admin@company.com");
            var client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            mail.Subject = "Commitment Posting Error";
            mail.Body = "The commitment " + commitmentGUID + " failed to post because " + message;
            client.Send(mail);
        }
    }
}
