using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.P2P.grsImageIface;
using System.Xml;
using System.Drawing;
using System.IO;

namespace PROACTIS.ExampleApplications.ExampleImagingTests
{
    [TestClass]
    public class UploadTests
    {

        [TestMethod]
        public void CheckWeCanUploadAnImage()
        {
            var reference = "example.bmp";
            var imageFolder = @"c:\temp";
            Directory.CreateDirectory(imageFolder);
            var targetFileName = Path.Combine(imageFolder, reference);

            // Create some meta data
            var detailsXML = GetDetailsXML(reference, imageFolder);

            // Create an image
            var bm = new Bitmap(100, 100);
            var g = Graphics.FromImage(bm);
            var red = 0;
            var white = 11;
            while (white <= 100)
            {
                g.FillRectangle(Brushes.Red, 0, red, 200, 10);
                g.FillRectangle(Brushes.White, 0, white, 200, 10);
                red += 20;
                white += 20;
            }
            var image = ImageToByte(bm);

            // Make sure that the image doesn't already exist
            File.Delete(targetFileName);

            // Act
            var service = new PROACTIS.ExampleApplications.ExampleImaging.Upload() as IUpload;
            var actualResult = service.StoreNewImage(detailsXML, image);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.IsTrue(File.Exists(targetFileName));
        }

        private static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        private static string GetDetailsXML(string reference, string imageFolder)
        {
            return $@"<?xml version='1.0'?>
<grs:ImagingSettings xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
    <grs:DocumentGUID>{{7451C54A-52F0-4794-87FB-A6093AAD65AF}}</grs:DocumentGUID>
    <grs:ImageNumber>1</grs:ImageNumber>

    <grs:SessionID>bd860b98-c82b-47fa-9522-a4f27f154cd6#dbserver2008r2\qa#DavidB_94#en-gb</grs:SessionID>
    <grs:DocumentType>I</grs:DocumentType>
    <grs:CompanyGUID>{{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}}</grs:CompanyGUID>
    <grs:Reference>{reference}</grs:Reference>
    <grs:FileType>BMP</grs:FileType>

    <grs:ImageFolder>{imageFolder}</grs:ImageFolder>
</grs:ImagingSettings>";
        }
    }
}
