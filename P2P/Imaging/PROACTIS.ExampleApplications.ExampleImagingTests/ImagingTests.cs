using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.P2P.grsImageIface;

namespace PROACTIS.ExampleApplications.ExampleImagingTests
{
    [TestClass]
    public class ImagingTests
    {
        [TestMethod]
        public void CheckWeHaveAnImage()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Imaging() as IImaging;
            var documentDetailsXML = GetDocumentDetailsXML();

            var actualResult = service.HasImage(documentDetailsXML);
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void CheckTheImageWillBeAURL()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Imaging() as IImaging;
            var documentDetailsXML = GetDocumentDetailsXML();

            var actualResult = service.GetImageInfo(documentDetailsXML, out var MIMEType);
            Assert.AreEqual(1, actualResult);
            Assert.AreEqual("text/url", MIMEType);
        }

        [TestMethod]
        public void CheckTheImageIsAURL()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Imaging() as IImaging;
            var documentDetailsXML = GetDocumentDetailsXML();

            var actualResult = service.GetImage(documentDetailsXML, out var MIMEType, out var image, out var URL);
            Assert.IsTrue(actualResult);
            Assert.AreEqual("text/url", MIMEType);
            Assert.IsFalse(string.IsNullOrWhiteSpace(URL));
            Assert.IsTrue(URL.StartsWith("http"));
        }

        private static string GetDocumentDetailsXML()
        {
            return @"<?xml version='1.0'?><grs:ImagingSettings xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
<grs:SessionID>eb89c444-0270-4f06-b8b6-ec0303b00117#dbserver2008r2\qa#DavidB_94#en-gb</grs:SessionID>
<grs:DocumentType>I</grs:DocumentType>
<grs:DocumentGUID>{D79D1EE8-4B87-414B-8512-92590DFBE2E8}</grs:DocumentGUID>
<grs:ImageNumber>0</grs:ImageNumber>
<grs:CompanyGUID>{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}</grs:CompanyGUID></grs:ImagingSettings>";
        }
    }
}
