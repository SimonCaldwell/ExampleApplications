using System;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    internal class SearchParameters
    {
        public string SessionID { get; private set; }
        public string DocumentType { get; private set; }
        public Guid CompanyGUID { get; private set; }
        public int MaxReturnRows { get; private set; }
        public string PrimarySortColumn { get; private set; }
        public bool PrimarySortAscending { get; private set; }
        public string SupplierReference { get; private set; }
        public string DateFrom { get; private set; }
        public string DateTo { get; private set; }
        public string FromAddress { get; private set; }
        public string ToAddress { get; private set; }
        public string Subject { get; private set; }
        public string RuleName { get; private set; }
        public string DateEmailedFrom { get; private set; }
        public string DateEmailedTo { get; private set; }

        private SearchParameters(string detailsXML)
        {
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

            var dom = new XmlDocument(nt);
            dom.LoadXml(detailsXML);

            // Core fields
            this.SessionID = dom.DocumentElement.SelectSingleNode("grs:SessionID", nsmgr).InnerText;
            this.DocumentType = dom.DocumentElement.SelectSingleNode("grs:DocumentType", nsmgr).InnerText;
            this.CompanyGUID =new Guid(dom.DocumentElement.SelectSingleNode("grs:CompanyGUID", nsmgr).InnerText);
            this.MaxReturnRows = int.Parse(dom.DocumentElement.SelectSingleNode("grs:MaxReturnRows", nsmgr).InnerText);
            this.PrimarySortColumn = dom.DocumentElement.SelectSingleNode("grs:PrimarySortColumn", nsmgr).InnerText;
            this.PrimarySortAscending = bool.Parse(dom.DocumentElement.SelectSingleNode("grs:PrimarySortAscending", nsmgr).InnerText);
            this.SupplierReference = dom.DocumentElement.SelectSingleNode("grs:SupplierReference", nsmgr).InnerText;
            this.DateFrom = dom.DocumentElement.SelectSingleNode("grs:DateFrom", nsmgr).InnerText;
            this.DateTo = dom.DocumentElement.SelectSingleNode("grs:DateTo", nsmgr).InnerText;
            this.FromAddress = dom.DocumentElement.SelectSingleNode("grs:FromAddress", nsmgr).InnerText;
            this.ToAddress = dom.DocumentElement.SelectSingleNode("grs:ToAddress", nsmgr).InnerText;
            this.Subject = dom.DocumentElement.SelectSingleNode("grs:Subject", nsmgr).InnerText;
            this.RuleName = dom.DocumentElement.SelectSingleNode("grs:RuleName", nsmgr).InnerText;
            this.DateEmailedFrom = dom.DocumentElement.SelectSingleNode("grs:DateEmailedFrom", nsmgr).InnerText;
            this.DateEmailedTo = dom.DocumentElement.SelectSingleNode("grs:DateEmailedTo", nsmgr).InnerText;

            // Add any custom fields here
        }



        internal static SearchParameters FromXML(string detailsXML)
        {
            return new SearchParameters(detailsXML);
        }
    }
}