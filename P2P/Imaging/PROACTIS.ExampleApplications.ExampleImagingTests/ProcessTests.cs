using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.P2P.grsImageIface;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImagingTests
{
    [TestClass]
    public class ProcessTests
    {
        [TestMethod]
        public void CheckWeHaveUnprocessedImages()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Process() as IProcess;
            var documentDetailsXML = GetDocumentDetailsXML();

            var actualResult = service.HasUnprocessedImages(documentDetailsXML);
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void CheckWeGetAListOfImagesToProcess()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Process() as IProcess;
            var documentDetailsXML = GetDocumentDetailsXML();

            var actualResult = service.GetListOfUnprocessedImages(documentDetailsXML);

            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");

            var dom = new XmlDocument(nt);
            dom.LoadXml(actualResult);

            var numberOfImages = dom.SelectNodes("grs:Images/grs:Image", nsmgr).Count;
            Assert.AreEqual(1, numberOfImages);
        }

        [TestMethod]
        public void CheckTheImageWillBeAURL()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Process() as IProcess;
            var documentDetailsXML = GetDocumentDetailsXML();

            var MIMEType = "";
            var actualResult = service.GetImageInfo(documentDetailsXML, ref MIMEType);
            Assert.AreEqual(1, actualResult);
            Assert.AreEqual("text/url", MIMEType);
        }

        [TestMethod]
        public void CheckTheImageIsAURL()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Process() as IProcess;
            var documentDetailsXML = GetDocumentDetailsXML();

            var MIMEType = "";
            var URL = "";
            var image = default(byte[]);
            var actualResult = service.GetImage(documentDetailsXML, ref MIMEType, ref image, ref URL);
            Assert.IsTrue(actualResult);
            Assert.AreEqual("text/url", MIMEType);
            Assert.IsFalse(string.IsNullOrWhiteSpace(URL));
            Assert.IsTrue(URL.StartsWith("http"));
        }

        [TestMethod]
        public void CheckWeCanProcessADocument()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Process() as IProcess;
            var documentDetailsXML = GetDocumentDetailsXML();

            var DocumentXML = @"<?xml version='1.0'?><grs:PurchaseInvoice xmlns:grs='http://www.getrealsystems.com/xml/xml-ns' grs:GUID='2d43d28b-687c-414d-939f-5e1983dee1bb'></grs:PurchaseInvoice>";
            var UserMessages = "";
            var actualResult = service.ProcessImage(documentDetailsXML, DocumentXML, ref UserMessages);
            Assert.IsTrue(actualResult);
        }

        private static string GetDocumentDetailsXML()
        {
            return @"<?xml version='1.0'?><grs:ImagingSettings xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'><grs:SessionID>2f87deb6-bb2a-45c9-93d9-e3aa8c4a32ae#dbserver2008r2\qa#DavidB_94#en-gb</grs:SessionID><grs:DocumentType>I</grs:DocumentType><grs:DocumentGUID>PINV123.xml</grs:DocumentGUID><grs:ImageNumber>0</grs:ImageNumber><grs:CompanyGUID>{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}</grs:CompanyGUID><grs:InvoiceImageIdentifier>DisplayNumber</grs:InvoiceImageIdentifier><grs:DefaultImageSource>URL</grs:DefaultImageSource><grs:DefaultURL>https://sp-db01/imaging/{{ImageID}}.bmp</grs:DefaultURL></grs:ImagingSettings>";
        }
    }
}
