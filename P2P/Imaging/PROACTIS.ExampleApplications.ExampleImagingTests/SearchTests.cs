using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.P2P.grsImageIface;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImagingTests
{
    [TestClass]
    public class SearchTests
    {

        [TestMethod]
        public void CheckWeCanSearchForAnImage()
        {
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Search() as ISearch;
            var detailsXML = GetDetailsXML();

            var actualResult = service.SearchForUnprocessedImages(detailsXML);


            var dom = new XmlDocument();
            dom.LoadXml(actualResult);

            var numberOfImages = dom.SelectNodes("SearchResults/Row").Count;
            Assert.AreEqual(1, numberOfImages);
        }

        [TestMethod]
        public void CheckAllTheRequiredFieldsAreReturned()
        {
            //Arrange
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Search() as ISearch;
            var detailsXML = GetDetailsXML();

            //Act
            var actualResult = service.SearchForUnprocessedImages(detailsXML);

            //Assert
            var dom = new XmlDocument();
            dom.LoadXml(actualResult);

            foreach (XmlElement row in dom.SelectNodes("SearchResults/Row"))
            {
                Assert.IsNotNull(row.GetAttributeNode("GUID"));
                Assert.IsNotNull(row.GetAttributeNode("SupplierReference"));
                Assert.IsNotNull(row.GetAttributeNode("DocumentDate"));
                Assert.IsNotNull(row.GetAttributeNode("DocumentImportRule"));
                Assert.IsNotNull(row.GetAttributeNode("EmailDate"));
                Assert.IsNotNull(row.GetAttributeNode("FromAddress"));
                Assert.IsNotNull(row.GetAttributeNode("FromName"));
                Assert.IsNotNull(row.GetAttributeNode("Subject"));
                Assert.IsNotNull(row.GetAttributeNode("Subject"));
                Assert.IsNotNull(row.GetAttributeNode("ToAddress"));
            }

        }

        private static string GetDetailsXML()
        {
            return @"<?xml version='1.0'?>
<grs:ImagingSettings xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
    <grs:SessionID>fd2ae334-dd29-42d3-9706-ea4883b7bedc#dbserver2008r2\qa#DavidB_94#en-gb</grs:SessionID>
    <grs:DocumentType>I</grs:DocumentType>
    <grs:DocumentGUID></grs:DocumentGUID>
    <grs:ImageNumber>0</grs:ImageNumber>
    <grs:CompanyGUID>{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}</grs:CompanyGUID>
    <grs:MaxReturnRows>100</grs:MaxReturnRows>
    <grs:PrimarySortColumn>DocumentDate</grs:PrimarySortColumn>
    <grs:PrimarySortAscending>False</grs:PrimarySortAscending>
    <grs:SupplierReference></grs:SupplierReference>
    <grs:DateFrom></grs:DateFrom>
    <grs:DateTo></grs:DateTo>
    <grs:FromAddress></grs:FromAddress>
    <grs:ToAddress></grs:ToAddress>
    <grs:Subject></grs:Subject>
    <grs:RuleName></grs:RuleName>
    <grs:DateEmailedFrom></grs:DateEmailedFrom>
    <grs:DateEmailedTo></grs:DateEmailedTo>
    <grs:DefaultImageSource>URL</grs:DefaultImageSource>
    <grs:DefaultURL>https://sp-db01/imaging/{{ImageID}}.bmp</grs:DefaultURL>
</grs:ImagingSettings>";
        }
    }
}
