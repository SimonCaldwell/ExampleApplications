using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROACTIS.P2P.grsImageIface;
using System.IO;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Process : P2P.grsImageIface.IProcess
    {
        bool IProcess.GetImage(string DetailsXML, out string MIMEType, out byte[] Image, out string URL)
        {
            var documentDetails = ProcessDetails.FromXML(DetailsXML);

            MIMEType = "text/url";
            URL = "https://sp-db01/Imaging/Invoice16732.bmp";
            Image = null;
            return true;
        }

        int IProcess.GetImageInfo(string DetailsXML, out string MIMEType)
        {
            var documentDetails = ProcessDetails.FromXML(DetailsXML);
            MIMEType = "text/url";
            return 1;
        }

        string IProcess.GetListOfUnprocessedImages(string DetailsXML)
        {
            var documentDetails = ProcessDetails.FromXML(DetailsXML);

            return @"<?xml version='1.0'?>
<grs:Images xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
    <grs:Image grs:Identifier='PINV123.xml'>
        <grs:Fields>
            <grs:Field grs:Name='Column1' grs:Value='Value in column 1' />
            <grs:Field grs:Name='Column2' grs:Value='Value in column 2' />
        </grs:Fields>
    </grs:Image>
</grs:Images>";
        }

        bool IProcess.HasUnprocessedImages(string DetailsXML)
        {
            var documentDetails = ProcessDetails.FromXML(DetailsXML);

            // We always show the link.
            return true;
        }

        bool IProcess.ProcessImage(string DetailsXML, string DocumentXML, out string UserMessages)
        {
            // Get the details of the image
            UserMessages = "";
            var documentDetails = ProcessDetails.FromXML(DetailsXML);
            var ourImage = documentDetails.DocumentGUID;

            // Get the GUID of the P2P document
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", "http://www.getrealsystems.com/xml/xml-ns");
            var dom = new XmlDocument(nt);
            dom.LoadXml(DocumentXML);
            var documentGUID = new Guid(dom.DocumentElement.SelectSingleNode("@grs:GUID",nsmgr).InnerText);

            // TODO: Remove the image from the original list of images returned by the call to GetListOfUnprocessedImages

            // TODO: Associate the image with the P2P document.  For example populate the ImageReference column,  or rename the
            // image to match the document's GUID/Number etc

            // Return TRUE to show that we have successfully linked the image to the document
            return true;
        }
    }
}
