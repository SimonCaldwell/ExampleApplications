using System;
using System.Xml;

namespace PROACTIS.ExampleApplication.CancelOrderByGUID
{
    class Program
    {
        static void Main(string[] args)
        {
            // All import gateways expect XML within the http://www.proactis.com/xml/xml-ns namespace
            const string NS = "http://www.proactis.com/xml/xml-ns";
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("pro", NS);

            // Create the xml document
            var authenticateUserDOM = new XmlDocument(nt);
            authenticateUserDOM.AppendChild(authenticateUserDOM.CreateElement("pro", "AuthenticateUser", NS));

            // Add the connection details
            var control = authenticateUserDOM.CreateElement("pro", "Control", NS);
            authenticateUserDOM.DocumentElement.AppendChild(control);
            control.SetAttribute("DatabaseName", "p2p");
            control.SetAttribute("UserName", "sysadmin");
            control.SetAttribute("Password", "a");
            control.SetAttribute("Company", "MAIN");
            control.SetAttribute("Version", "1.0.0");
            var ControlXML = authenticateUserDOM.OuterXml;

            // Identify the order we wish to cancel
            var OrderGUID = new Guid("A2748707-F329-4435-8420-CB3920567ABF");
            var CancellationReason = "Problems in supply";
            var Comments = "Supplier can no longer deliver living Dodo birds.";

            var ws = new p2p.XMLGatewaySoapClient();
            ws.CancelOrderByGUID(ControlXML, OrderGUID.ToString(), CancellationReason, Comments);

        }
    }
}
