using PROACTIS.P2P.grsImageIface;

namespace PROACTIS.ExampleApplications.ExampleImaging
{
    public class Search : P2P.grsImageIface.ISearch
    {
        string ISearch.SearchForUnprocessedImages(string DetailsXML)
        {
            var searchParameters = SearchParameters.FromXML(DetailsXML);

            // Return one row per image
            return @"<SearchResults>
                        <Row GUID='guid' 
                             SupplierReference='sup' 
                             DocumentDate='2016-1-1' 
                             DocumentImportRule = ''
                             EmailDate='2016-1-1'
                             FromAddress='david@proactis.com'
                             FromName='David Betteridge'
                             Subject='An Email'
                             ToAddress='david@proactis.com' />
                     </SearchResults>";
        }

    }
}
