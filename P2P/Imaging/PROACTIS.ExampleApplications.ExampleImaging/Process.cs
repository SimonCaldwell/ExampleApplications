using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROACTIS.P2P.grsImageIface;
using System.IO;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Process : P2P.grsImageIface.IProcess
    {
        bool IProcess.GetImage(string DetailsXML, ref string MIMEType, ref byte[] Image, ref string URL)
        {
            MIMEType = "text/url";
            URL = "https://sp-db01/Imaging/Invoice16732.bmp";
            return true;
        }

        int IProcess.GetImageInfo(string DetailsXML, ref string MIMEType)
        {
            MIMEType = "text/url";
            return 1;
        }

        string IProcess.GetListOfUnprocessedImages(string DetailsXML)
        {
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
            File.WriteAllText(@"c:\temp\Process_HasUnprocessedImages.xml", DetailsXML);
            return true;
        }

        bool IProcess.ProcessImage(string DetailsXML, string DocumentXML, ref string UserMessages)
        {
            File.WriteAllText(@"c:\temp\Process_ProcessImage_DetailsXML.xml", DetailsXML);
            File.WriteAllText(@"c:\temp\Process_ProcessImage_DocumentXML.xml", DocumentXML);
            return true;
        }
    }
}
