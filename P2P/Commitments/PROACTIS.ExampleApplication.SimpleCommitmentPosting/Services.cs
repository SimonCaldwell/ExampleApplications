using System;
using System.IO;
using PROACTIS.P2P.grsCustInterfaces;

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
            var filename = Path.Combine(@"c:\temp", commitmentGUID.ToString() + ".xml");
            File.WriteAllText(filename, commitmentXML);
        }

    }
}
