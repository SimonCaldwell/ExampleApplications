﻿using PROACTIS.P2P.grsImageIface;
using System.IO;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Imaging : P2P.grsImageIface.IImaging
    {
        bool IImaging.GetImage(string DocumentDetailsXML, out string MIMEType, out byte[] Image, out string URL)
        {
            var documentDetails = ImagingDetails.FromXML(DocumentDetailsXML);

            // We are going to return a link to an image,  rather than the image itself.
            MIMEType = "text/url";
            URL = "https://server/image.bmp";
            Image = null;
            return true;
        }
        
        int IImaging.GetImageInfo(string DocumentDetailsXML, out string MIMEType)
        {
            var documentDetails = ImagingDetails.FromXML(DocumentDetailsXML);

            // We are going to return a link to an image,  rather than the image itself.
            MIMEType = "text/url";
            return 1;
        }

        bool IImaging.HasImage(string DocumentDetailsXML)
        {
            var documentDetails = ImagingDetails.FromXML(DocumentDetailsXML);

            // We pretend there is always an image
            return true;
        }
    }
}
