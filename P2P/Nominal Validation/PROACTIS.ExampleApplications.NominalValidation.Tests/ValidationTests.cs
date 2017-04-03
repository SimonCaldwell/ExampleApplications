/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROACTIS.ExampleApplications.ExampleNominalValidation;

namespace PROACTIS.ExampleApplications.NominalValidation.Tests
{
    [TestClass]
    public class ValidationTests
    {
        private const string DATABASE = "PROACTIS";
        private const string DATABASE_SERVER = "localhost";

        /// <summary>
        /// All nominals are valid as for our sample element1 all start with an 'A'
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        public void CheckWhenAllNominalsAreValidASuccessIsRecord()
        {
            var service = new Services();
            var POXML = @"";
            var NominalsXML = $@"           
               <grs:NominalCheck xmlns:grs=""http://www.getrealsystems.com/xml/xml-ns"">
               <grs:Database grs:Server=""{DATABASE_SERVER}"" grs:DatabaseName=""{DATABASE}""/>
               <grs:General grs:UserGUID=""{{02E0D6D9-B655-11D5-91D6-000629864A98}}"" grs:CompanyGUID=""{{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}}""/>
               <grs:Nominals>
                  <grs:Nominal grs:Coding=""A"" grs:Element1=""A"" grs:Element2="""" grs:Element3="""" grs:Element4="""" grs:Element5="""" grs:Element6="""" grs:Element7="""" grs:Element8="""" grs:ValidNominal=""False""/>
                  <grs:Nominal grs:Coding=""A.1100"" grs:Element1=""A"" grs:Element2=""1100"" grs:Element3="""" grs:Element4="""" grs:Element5="""" grs:Element6="""" grs:Element7="""" grs:Element8="""" grs:ValidNominal=""False""/>
               </grs:Nominals>
               </grs:NominalCheck>";
            var ErrorNominals = "";

            var actualResult = service.NominalCheck(NominalsXML, POXML, ref ErrorNominals);
            Assert.IsTrue(actualResult);
        }


        /// <summary>
        /// The second nominal is not valid as element1 starts with a B not an A
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        public void CheckWhenANominalIsInvalidAFailureIsRecord()
        {
            var service = new Services();
            var POXML = @"";
            var NominalsXML = $@"           
               <grs:NominalCheck xmlns:grs=""http://www.getrealsystems.com/xml/xml-ns"">
               <grs:Database grs:Server=""{DATABASE_SERVER}"" grs:DatabaseName=""{DATABASE}""/>
               <grs:General grs:UserGUID=""{{02E0D6D9-B655-11D5-91D6-000629864A98}}"" grs:CompanyGUID=""{{A2FEEDC5-978F-11D5-8C5E-0001021ABF9B}}""/>
               <grs:Nominals>
                  <grs:Nominal grs:Coding=""A"" grs:Element1=""A"" grs:Element2="""" grs:Element3="""" grs:Element4="""" grs:Element5="""" grs:Element6="""" grs:Element7="""" grs:Element8="""" grs:ValidNominal=""False""/>
                  <grs:Nominal grs:Coding=""BBBB.1100"" grs:Element1=""BBB"" grs:Element2=""1100"" grs:Element3="""" grs:Element4="""" grs:Element5="""" grs:Element6="""" grs:Element7="""" grs:Element8="""" grs:ValidNominal=""False""/>
               </grs:Nominals>
               </grs:NominalCheck>";
            var ErrorNominals = "";

            var actualResult = service.NominalCheck(NominalsXML, POXML, ref ErrorNominals);
            Assert.IsFalse(actualResult);
        }

    }
}
