using System;
using PROACTIS.P2P.grsCustInterfaces;
using System.IO;

namespace PROACTIS.ExampleApplication.SimpleExportProcessor
{
    public class Services : IExportProcessor
    {
        ExportProcessorInitialiseResult IExportProcessor.Initialise(int numberOfDocuments, string databaseTitle, string databaseName, string databaseServerName)
        {
            // Each document will be written to it's own file,  so each document can be in it's own transaction.
            return ExportProcessorInitialiseResult.OneTransactionPerDocument;
        }

        ExportProcessorExportResult IExportProcessor.ProcessDocument(Guid guid, string documentNumber, string documentXml, Guid documentGuid, string documentType, string description)
        {
            var filename = Path.Combine(@"c:\temp", documentNumber + ".xml");
            File.WriteAllText(filename, documentXml);

            return ExportProcessorExportResult.Success;
        }

        void IExportProcessor.PostingComplete()
        {
            // Nothing to do
        }
    }
}
