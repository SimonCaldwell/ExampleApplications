using PROACTIS.P2P.grsImageIface;
using System.IO;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Imaging : P2P.grsImageIface.IImaging
    {
        bool IImaging.GetImage(string DocumentDetailsXML, ref string MIMEType, ref byte[] Image, ref string URL)
        {
            File.WriteAllText(@"c:\temp\GetImage.xml", DocumentDetailsXML);
            return true;
        }

        int IImaging.GetImageInfo(string DocumentDetailsXML, ref string MIMEType)
        {
            File.WriteAllText(@"c:\temp\GetImageInfo.xml", DocumentDetailsXML);
            return 0;
        }

        bool IImaging.HasImage(string DocumentDetailsXML)
        {
            File.WriteAllText(@"c:\temp\HasImage.xml", DocumentDetailsXML);
            return true;
        }
    }
}
