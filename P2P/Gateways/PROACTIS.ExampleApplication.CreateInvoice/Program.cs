using System;
using System.Xml;

namespace PROACTIS.ExampleApplication.CreateInvoice
{
    class Program
    {
        static void Main(string[] args)
        {
            // All import gateways expect XML within this namespace
            const string NS = "http://www.proactis.com/xml/xml-ns";
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("pro", NS);

            // Create the xml document
            var dom = new XmlDocument(nt);
            dom.AppendChild(dom.CreateElement("pro", "Import", NS));

            // Add the connection details
            var control = dom.CreateElement("pro", "Control", NS);
            dom.DocumentElement.AppendChild(control);
            control.SetAttribute("DatabaseName", "p2p");
            control.SetAttribute("UserName", "sysadmin");
            control.SetAttribute("Password", "a");
            control.SetAttribute("Company", "MAIN");
            control.SetAttribute("Version", "1.0.0");
            control.SetAttribute("ErrorHandlingMode", "EMBED");
            var ControlXML = dom.OuterXml;

            // Create the document
            var invoice = dom.CreateElement("pro", "Invoice", NS);
            dom.DocumentElement.AppendChild(invoice);
            invoice.SetAttribute("InvoiceDate", DateTime.Now.ToString("yyyy-MM-dd"));
            invoice.SetAttribute("SupplierInvoiceNumber", "MyInvoice " + Guid.NewGuid().ToString());
            invoice.SetAttribute("Template", "SINV");
            invoice.SetAttribute("Tray", "Fully Matched");
            invoice.SetAttribute("GrossValue", "120");
            invoice.SetAttribute("TaxValue", "20");
            invoice.SetAttribute("Supplier", "CAR02");

            // Add a standalone item
            var nonOrderItems = dom.CreateElement("pro", "NonOrderItems", NS);
            invoice.AppendChild(nonOrderItems);

            var nonOrderItem = dom.CreateElement("pro", "NonOrderItem", NS);
            nonOrderItems.AppendChild(nonOrderItem);
            nonOrderItem.SetAttribute("SelectUsingPROACTISCode", "EL008");
            nonOrderItem.SetAttribute("NetValue", "120");

            // And it's nominals
            var nonOrderItemNominals = dom.CreateElement("pro", "NonOrderItemNominals", NS);
            nonOrderItem.AppendChild(nonOrderItemNominals);

            var nonOrderItemNominal = dom.CreateElement("pro", "NonOrderItemNominal", NS);
            nonOrderItemNominals.AppendChild(nonOrderItemNominal);
            nonOrderItemNominal.SetAttribute("AccountingElement3", "GR-1");

            // Import the document
            var ws = new p2p.XMLGatewaySoapClient();

            try
            {
                var result = ws.ImportDocument(dom.OuterXml);

                // Parse the results to get the details of the created invoice
                var resultDom = new XmlDocument(nt);
                resultDom.LoadXml(result);

                var resultInvoice = (XmlElement)resultDom.DocumentElement.SelectSingleNode("pro:Invoice", nsmgr);
                var status = resultInvoice.GetAttribute("Status");

                if (status != "OK")
                {
                    // We were expecting the invoice to be created.  If the error handling mode has been 
                    // set to EMBED,  then we need to extract any error messages from the returned xml
                    var errorMessage = GetErrorTextFromResult(result);
                }

                var documentNumber = resultInvoice.GetAttribute("DocumentNumber");
                var documentGUID = resultInvoice.GetAttribute("GUID");
            }

            catch (Exception ex)
            {
                // If the error handling mode has been set to THROWTEXT then message text of the
                // thrown exception should contain a readable error message.
                var errMessage = ex.Message;

                // If the error handling mode has been set to THROWXML then you will need to extract
                // the error message from within the body of the xml
                // var errMessage = GetErrorTextFromResult(ex.Message);

                // If the error handling mode has been set to THROWERRORS then you will need to extract
                // the error message from within the body of the errors xml
                // var errMessage = GetErrorTextFromErrorsXML(ex.Message);


                throw;
            }

        }

        /// <summary>
        /// Used to parse to either errors thrown when the error handling mode is set to THROWXML
        /// or the xml returned by the web service when the error handling mode is set to EMBED
        /// </summary>
        /// <param name="returnedXML"></param>
        /// <returns></returns>
        private static string GetErrorTextFromResult(string returnedXML)
        {
            const string NS = "http://www.proactis.com/xml/xml-ns";
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("pro", NS);

            var resultDom = new XmlDocument(nt);
            resultDom.LoadXml(returnedXML);

            var errorMessage = string.Empty;
            foreach (XmlElement error in resultDom.SelectNodes("//pro:Error", nsmgr))
                errorMessage += error.GetAttribute("Message");

            return errorMessage;
        }

        /// <summary>
        /// Used to parse errors thrown when the error handling mode is set to THROWERRORS
        /// </summary>
        /// <param name="returnedXML"></param>
        /// <returns></returns>
        private static string GetErrorTextFromErrorsXML(string returnedXML)
        {
            const string NS = "http://www.proactis.com/xml/xml-ns/Errors";
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("pro", NS);

            var resultDom = new XmlDocument(nt);
            resultDom.LoadXml(returnedXML);

            var errorMessage = string.Empty;
            foreach (XmlElement error in resultDom.SelectNodes("//pro:Error", nsmgr))
                errorMessage += error.GetAttribute("Message");

            return errorMessage;
        }
    }
}
