using System;
using System.Xml;

namespace PROACTIS.ExampleApplication.CreateInternalInvoice
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
            var internalInvoice = dom.CreateElement("pro", "InternalInvoice", NS);
            dom.DocumentElement.AppendChild(internalInvoice);

            ////////////////////////////////////////////////////////////////////
            // Add the order item
            var orderItems = dom.CreateElement("pro", "OrderItems", NS);
            internalInvoice.AppendChild(orderItems);
            orderItems.SetAttribute("OrderNumber", "PORD10035");

            var orderItem = dom.CreateElement("pro", "OrderItem", NS);
            orderItems.AppendChild(orderItem);
            orderItem.SetAttribute("Position", "1");
            orderItem.SetAttribute("Price", "7.99");
            orderItem.SetAttribute("Quantity", "5");
            orderItem.SetAttribute("Description", "Test Internal Order Item");

            // And it's nominals
            var orderNominals = dom.CreateElement("pro", "OrderNominals", NS);
            orderItem.AppendChild(orderNominals);

            var orderNominal = dom.CreateElement("pro", "OrderNominal", NS);
            orderNominals.AppendChild(orderNominal);
            orderNominal.SetAttribute("Code", "ABC.123");
            orderNominal.SetAttribute("Quantity", "3");
            orderNominal.SetAttribute("Price", "7.99");

            ////////////////////////////////////////////////////////////////////
            // Add the invoice item
            var invoiceItems = dom.CreateElement("pro", "InvoiceItems", NS);
            internalInvoice.AppendChild(invoiceItems);

            var invoiceItem = dom.CreateElement("pro", "InvoiceItem", NS);
            invoiceItems.AppendChild(invoiceItem);
            invoiceItem.SetAttribute("BuyerItemCode", "ABC");
            invoiceItem.SetAttribute("SupplierItemCode", "123");
            invoiceItem.SetAttribute("Price", "5");
            invoiceItem.SetAttribute("Quantity", "5");
            invoiceItem.SetAttribute("Description", "Test Internal Order Item");

            // And it's nominals
            var invoiceNominals = dom.CreateElement("pro", "InvoiceNominals", NS);
            invoiceItem.AppendChild(invoiceNominals);

            var invoiceNominal = dom.CreateElement("pro", "InvoiceNominal", NS);
            invoiceNominals.AppendChild(invoiceNominal);
            invoiceNominal.SetAttribute("Code", "ABC.123");
            invoiceNominal.SetAttribute("Value", "3");

            ////////////////////////////////////////////////////////////////////
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
