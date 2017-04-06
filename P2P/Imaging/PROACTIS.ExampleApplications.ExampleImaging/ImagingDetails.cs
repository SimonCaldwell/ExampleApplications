using System;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    internal class ImagingDetails
    {
        public string SessionID { get; private set; }
        public string DocumentType { get; private set; }
        public Guid DocumentGUID { get; private set; }
        public int ImageNumber { get; private set; }
        public Guid CompanyGUID { get; private set; }

        private ImagingDetails(string documentDetailsXML)
        {
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

            var dom = new XmlDocument(nt);
            dom.LoadXml(documentDetailsXML);

            // Core fields
            this.SessionID = dom.DocumentElement.SelectSingleNode("grs:SessionID", nsmgr).InnerText;
            this.DocumentType = dom.DocumentElement.SelectSingleNode("grs:DocumentType", nsmgr).InnerText;
            this.DocumentGUID = new Guid(dom.DocumentElement.SelectSingleNode("grs:DocumentGUID", nsmgr).InnerText);
            this.ImageNumber = int.Parse(dom.DocumentElement.SelectSingleNode("grs:ImageNumber", nsmgr).InnerText);
            this.CompanyGUID = new Guid(dom.DocumentElement.SelectSingleNode("grs:CompanyGUID", nsmgr).InnerText);

            // Add your custom fields (defined in dsdba.ImagingSettings) in here
            //this.RootURL = dom.DocumentElement.SelectSingleNode("grs:RootURL", nsmgr).InnerText;
        }

        internal static ImagingDetails FromXML(string documentDetailsXML)
        {
            return new ImagingDetails(documentDetailsXML);
        }
    }
}


