using PROACTIS.P2P.grsCustInterfaces;
using System;
using System.IO;
using System.Text;

namespace PROACTIS.ExampleApplication.BatchExportProcessor
{
    public class Services : IExportProcessor
    {
        private StringBuilder sb;

        ExportProcessorInitialiseResult IExportProcessor.Initialise(int numberOfDocuments, string databaseTitle, string databaseName, string databaseServerName)
        {
            // We are build up our flat file within the string builder and only write it disk at the last moment.
            this.sb = new StringBuilder();

            // Our file starts with a header line stating the number of documents we are going to include
            this.sb.AppendLine("Number of documents :" + numberOfDocuments);

            // We want to roll back the entire export if we hit a problem.
            return ExportProcessorInitialiseResult.SingleTransactionForAllDocuments;
        }

        ExportProcessorExportResult IExportProcessor.ProcessDocument(Guid guid, string documentNumber, string documentXml, Guid documentGuid, string documentType, string description)
        {
            // Add the details of this invoice to the file.  Normally we would be pulling values out of the documentXml at this point.
            this.sb.AppendLine(documentNumber);

            //throw new Exception("ProcessDocument - transactions example");

            return ExportProcessorExportResult.SuccessButDoNotMarkAsPosted ;
        }

        void IExportProcessor.PostingComplete()
        {
            // Write the footer to the file
            this.sb.AppendLine("End_of_file");

            //throw new Exception("PostingComplete - transactions example");

            // Save the file to disk
            var filename = Path.Combine(@"c:\temp","Invoices.txt");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, this.sb.ToString());
        }
    }
}
