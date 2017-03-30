using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleNominalValidation
{
    internal class ObjectModel
    {
        private const string NS = "http://www.getrealsystems.com/xml/xml-ns";
        internal static Details FromXml(string nominalsXML)
        {

            // Load the xml
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", NS);

            var dom = new XmlDocument(nt);
            dom.LoadXml(nominalsXML);

            // Populate an object model
            var details = new Details();

            var database = (XmlElement)dom.DocumentElement.SelectSingleNode("grs:Database", nsmgr);
            if (database == null) throw new Exception("XML does not contain an element called database.");
            details.DatabaseServer = database.GetAttribute("Server", NS);
            details.Database = database.GetAttribute("DatabaseName", NS);

            var general = (XmlElement)dom.DocumentElement.SelectSingleNode("grs:General", nsmgr);
            if (general == null) throw new Exception("XML does not contain an element called general.");
            details.UserGUID = general.GetAttribute("UserGUID", NS);
            details.CompanyGUID = general.GetAttribute("CompanyGUID", NS);

            details.Nominals = new List<Nominal>();
            foreach (XmlElement nom in dom.DocumentElement.SelectNodes("grs:Nominals/grs:Nominal", nsmgr))
            {
                var nominal = new Nominal();
                details.Nominals.Add(nominal);

                nominal.Coding = nom.GetAttribute("Coding", NS);
                nominal.Element1 = nom.GetAttribute("Element1", NS);
                nominal.Element2 = nom.GetAttribute("Element2", NS);
                nominal.Element3 = nom.GetAttribute("Element3", NS);
                nominal.Element4 = nom.GetAttribute("Element4", NS);
                nominal.Element5 = nom.GetAttribute("Element5", NS);
                nominal.Element6 = nom.GetAttribute("Element6", NS);
                nominal.Element7 = nom.GetAttribute("Element7", NS);
                nominal.Element8 = nom.GetAttribute("Element8", NS);
            }

            return details;
        }

        internal static string ToXml(Details details)
        {
            // Create a new DOM
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", NS);

            // Add any failed nominals to the xml
            var dom = new XmlDocument(nt);
            dom.AppendChild(dom.CreateElement("grs:Nominals", NS));

            foreach (var nom in details.Nominals.Where(n => !n.IsValid))
            {
                var nominal = dom.CreateElement("grs:Nominal", NS);
                dom.DocumentElement.AppendChild(nominal);

                nominal.SetAttribute("Coding", NS, nom.Coding);
                nominal.SetAttribute("Element1", NS, nom.Element1);
                nominal.SetAttribute("Element2", NS, nom.Element2);
                nominal.SetAttribute("Element3", NS, nom.Element3);
                nominal.SetAttribute("Element4", NS, nom.Element4);
                nominal.SetAttribute("Element5", NS, nom.Element5);
                nominal.SetAttribute("Element6", NS, nom.Element6);
                nominal.SetAttribute("Element7", NS, nom.Element7);
                nominal.SetAttribute("Element8", NS, nom.Element8);
                nominal.SetAttribute("ValidNominal", NS, "False");
            }

            return dom.OuterXml;
        }
    }

    internal class Nominal
    {
        public string Coding { get; internal set; }
        public string Element1 { get; internal set; }
        public string Element2 { get; internal set; }
        public string Element3 { get; internal set; }
        public string Element4 { get; internal set; }
        public string Element5 { get; internal set; }
        public string Element6 { get; internal set; }
        public string Element7 { get; internal set; }
        public string Element8 { get; internal set; }
        public bool IsValid { get; set; }
        public Nominal()
        {
            this.IsValid = true;
        }
    }

    internal class Details
    {
        public string DatabaseServer { get; internal set; }
        public string Database { get; internal set; }
        public string UserGUID { get; internal set; }
        public string CompanyGUID { get; internal set; }
        public List<Nominal> Nominals { get; internal set; }
    }
}