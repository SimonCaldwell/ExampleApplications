using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.P2P.grsImageIface;

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


        private static string GetDocumentDetailsXML()
        {
            return @"<?xml version='1.0'?><grs:ImagingSettings xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'><grs:SessionID>2f87deb6-bb2a-45c9-93d9-e3aa8c4a32ae#dbserver2008r2\qa#DavidB_94#en-gb</grs:SessionID><grs:DocumentType>I</grs:DocumentType><grs:DocumentGUID>PINV123.xml</grs:DocumentGUID><grs:ImageNumber>0</grs:ImageNumber><grs:CompanyGUID>{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}</grs:CompanyGUID><grs:InvoiceImageIdentifier>DisplayNumber</grs:InvoiceImageIdentifier><grs:DefaultImageSource>URL</grs:DefaultImageSource><grs:DefaultURL>https://sp-db01/imaging/{{ImageID}}.bmp</grs:DefaultURL></grs:ImagingSettings>";
        }
    }
}
