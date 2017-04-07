using System;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    internal class UploadDetails
    {
        public Guid DocumentGUID { get; private set; }
        public string SessionID { get; private set; }
        public string DocumentType { get; private set; }
        public Guid CompanyGUID { get; private set; }
        public string Reference { get; private set; }
        public string FileType { get; private set; }
        
        // Custom fields
        public string ImageFolder { get; private set; }
        
        private UploadDetails(string detailsXML)
        {
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

            var dom = new XmlDocument(nt);
            dom.LoadXml(detailsXML);

            // Core fields
            this.DocumentGUID = new Guid(dom.DocumentElement.SelectSingleNode("grs:DocumentGUID", nsmgr).InnerText);

            this.SessionID = dom.DocumentElement.SelectSingleNode("grs:SessionID", nsmgr).InnerText;
            this.DocumentType = dom.DocumentElement.SelectSingleNode("grs:DocumentType", nsmgr).InnerText;
            this.CompanyGUID = new Guid(dom.DocumentElement.SelectSingleNode("grs:CompanyGUID", nsmgr).InnerText);
            this.Reference = dom.DocumentElement.SelectSingleNode("grs:Reference", nsmgr).InnerText;
            this.FileType = dom.DocumentElement.SelectSingleNode("grs:FileType", nsmgr).InnerText;

            // Add any custom fields here
            this.ImageFolder = dom.DocumentElement.SelectSingleNode("grs:ImageFolder", nsmgr).InnerText;

        }
        internal static UploadDetails FromXML(string detailsXML)
        {
            return new UploadDetails(detailsXML);
        }
    }
}