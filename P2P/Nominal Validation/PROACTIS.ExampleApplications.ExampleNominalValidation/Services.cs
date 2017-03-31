/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
namespace PROACTIS.ExampleApplications.ExampleNominalValidation
{
    public class Services : P2P.grsCustInterfaces.INominals
    {
        public bool NominalCheck(string NominalsXML, string POXML, ref string ErrorNominals)
        {
            // Convert the supplied nominals xml into a object model
            var details = ObjectModel.FromXml(NominalsXML);

            // Check each nominal in turn
            var allValid = true;  //Assume the best
            foreach (var nominal in details.Nominals)
            {
                /*******************************
                 * 
                 *     Place your code here
                 * 
                 ******************************/
                if (!nominal.Element1.StartsWith("A"))
                {
                    // If this nominal is not valid then...
                    allValid = false;
                    nominal.IsValid = false;
                }
            }

            if (!allValid)
            {
                // At least one nominal has failed validation,  so we need to return the failures
                ErrorNominals = ObjectModel.ToXml(details);
            }

            return allValid;
        }
    }
}
