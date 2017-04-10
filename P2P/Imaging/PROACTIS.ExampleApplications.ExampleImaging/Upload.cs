using System;
using PROACTIS.P2P.grsImageIface;
using System.IO;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Upload : P2P.grsImageIface.IUpload
    {

        bool IUpload.StoreNewImage(string DetailsXML, byte[] FileContents)
        {
            // Read the meta-data about the image we have been asked to upload
            var uploadDetails = UploadDetails.FromXML(DetailsXML);

            // In this example we are just going to write files to the temp folder
            var targetFileName = Path.Combine(uploadDetails.ImageFolder, uploadDetails.Reference);

            // Write the file
            File.WriteAllBytes(targetFileName, FileContents);

            // True for success
            return true;
        }
    }
}
