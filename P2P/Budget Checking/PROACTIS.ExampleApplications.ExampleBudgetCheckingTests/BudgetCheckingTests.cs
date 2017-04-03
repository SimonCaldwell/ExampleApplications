/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PROACTIS.ExampleApplications.ExampleBudgetCheckingTests
{
    [TestClass]
    public class BudgetCheckingTests
    {
        [TestMethod]
        [TestCategory("API")]
        public void WhenAllLinesAreLessThan100TheCheckShouldPass()
        {
            var service = new PROACTIS.ExampleApplications.ExampleBudgetChecking.Services();
            var POXML = "";
            var NominalsXML = @"<grs:CommitmentLookup xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
<grs:Database grs:Server='localhost' grs:DatabaseName='PROACTIS' />
<grs:General grs:UserGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:CompanyGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' />
<grs:Currencies><grs:Currency grs:CurrencyGUID='{2E67C438-9012-415B-AED4-8809F0012A78}' grs:Status='H1' grs:Symbol='£' grs:DecimalPlaces='2' /></grs:Currencies>

<grs:NominalPeriods>
    <grs:NominalPeriod grs:Year='2017' grs:Period='1' grs:YearPeriodGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:Value='98.43' 
    grs:Home1Value='98.43' grs:Home2Value='98.43' grs:NonRecoverableTax='0' grs:NonRecoverableTaxHome1='0' grs:NonRecoverableTaxHome2='0'>
        <grs:Nominal grs:Coding='SALES.CONF.MARKET' grs:Element1='SALES' grs:Element2='CONF' grs:Element3='MARKET' grs:Element4='' 
        grs:Element5='' grs:Element6='' grs:Element7='' grs:Element8=''></grs:Nominal>
    </grs:NominalPeriod>
</grs:NominalPeriods>

</grs:CommitmentLookup>";
            var actualResult = service.CommitmentCheck(NominalsXML, POXML);
            Assert.IsTrue(actualResult);
        }


        [TestMethod]
        [TestCategory("API")]
        public void When2LinesExceedTheBudgetThenTheOverspendShouldBeTheOverspendFromTheTwoLinesCombined()
        {
            var service = new PROACTIS.ExampleApplications.ExampleBudgetChecking.Services();
            var POXML = "";
            var expectedResult = 20M + 30M;
            var NominalsXML = @"<grs:CommitmentLookup xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
<grs:Database grs:Server='localhost' grs:DatabaseName='PROACTIS' />
<grs:General grs:UserGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:CompanyGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' />
<grs:Currencies><grs:Currency grs:CurrencyGUID='{2E67C438-9012-415B-AED4-8809F0012A78}' grs:Status='H1' grs:Symbol='£' grs:DecimalPlaces='2' /></grs:Currencies>

<grs:NominalPeriods>
    <grs:NominalPeriod grs:Year='2017' grs:Period='1' grs:YearPeriodGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:Value='130' 
    grs:Home1Value='130' grs:Home2Value='130' grs:NonRecoverableTax='0' grs:NonRecoverableTaxHome1='0' grs:NonRecoverableTaxHome2='0'>
        <grs:Nominal grs:Coding='SALES.CONF.MARKET' grs:Element1='SALES' grs:Element2='CONF' grs:Element3='MARKET' grs:Element4='' 
        grs:Element5='' grs:Element6='' grs:Element7='' grs:Element8=''></grs:Nominal>
    </grs:NominalPeriod>

    <grs:NominalPeriod grs:Year='2017' grs:Period='1' grs:YearPeriodGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:Value='120' 
    grs:Home1Value='120' grs:Home2Value='120' grs:NonRecoverableTax='0' grs:NonRecoverableTaxHome1='0' grs:NonRecoverableTaxHome2='0'>
        <grs:Nominal grs:Coding='SALES.CONF.MARKET' grs:Element1='SALES' grs:Element2='CONF' grs:Element3='MARKET' grs:Element4='' 
        grs:Element5='' grs:Element6='' grs:Element7='' grs:Element8=''></grs:Nominal>
    </grs:NominalPeriod>

</grs:NominalPeriods>

</grs:CommitmentLookup>";
            var actualResult = service.GetOverspend(NominalsXML, POXML);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [TestCategory("API")]
        public void When2LinesExceedTheBudgetThenACommitmentReportIsGenerated()
        {
            var service = new PROACTIS.ExampleApplications.ExampleBudgetChecking.Services();
            var POXML = "";

            var expectedResult = @"<grs:HeadedList xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'><grs:Headings><grs:Column grs:Number='1' grs:Type='' grs:BudgetType=''>Nominal Coding</grs:Column><grs:Column grs:Number='2' grs:Type='Currency' grs:BudgetType='Budget'>Budget For Year</grs:Column><grs:Column grs:Number='3' grs:Type='Currency' grs:BudgetType='Cost'>Spend To Date</grs:Column><grs:Column grs:Number='4' grs:Type='Currency' grs:BudgetType='Cost'>Accruals</grs:Column><grs:Column grs:Number='5' grs:Type='Currency' grs:BudgetType='Cost'>This Document</grs:Column><grs:Column grs:Number='6' grs:Type='Currency' grs:BudgetType='Cost'>Remaining Budget</grs:Column><grs:Column grs:Number='7' grs:Type='Highlight'>Highlight</grs:Column></grs:Headings><grs:Items><grs:Item><grs:Column grs:Type='' grs:Number='1'>SALES.CONF.MARKET</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='2'>10000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='3'>6000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='4'>1000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='5'>130</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:HyperLink='http://www.proactis.com' grs:Number='6'>2870</grs:Column><grs:Column grs:Type='Highlight' grs:Number='7'>False</grs:Column></grs:Item><grs:Item><grs:Column grs:Type='' grs:Number='1'>SALES.CONF.MARKET</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='2'>10000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='3'>6000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='4'>1000</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:Number='5'>120</grs:Column><grs:Column grs:Type='Currency' grs:CurrencySymbol='£' grs:DecimalPlaces='2' grs:HyperLink='http://www.proactis.com' grs:Number='6'>2880</grs:Column><grs:Column grs:Type='Highlight' grs:Number='7'>False</grs:Column></grs:Item></grs:Items></grs:HeadedList>";


            var NominalsXML = @"<grs:CommitmentLookup xmlns:grs='http://www.getrealsystems.com/xml/xml-ns'>
<grs:Database grs:Server='localhost' grs:DatabaseName='PROACTIS' />
<grs:General grs:UserGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:CompanyGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' />
<grs:Currencies><grs:Currency grs:CurrencyGUID='{2E67C438-9012-415B-AED4-8809F0012A78}' grs:Status='H1' grs:Symbol='£' grs:DecimalPlaces='2' /></grs:Currencies>

<grs:NominalPeriods>
    <grs:NominalPeriod grs:Year='2017' grs:Period='1' grs:YearPeriodGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:Value='130' 
    grs:Home1Value='130' grs:Home2Value='130' grs:NonRecoverableTax='0' grs:NonRecoverableTaxHome1='0' grs:NonRecoverableTaxHome2='0'>
        <grs:Nominal grs:Coding='SALES.CONF.MARKET' grs:Element1='SALES' grs:Element2='CONF' grs:Element3='MARKET' grs:Element4='' 
        grs:Element5='' grs:Element6='' grs:Element7='' grs:Element8=''></grs:Nominal>
    </grs:NominalPeriod>

    <grs:NominalPeriod grs:Year='2017' grs:Period='1' grs:YearPeriodGUID='{3A8D2AC2-6287-41DF-817A-F77B0551D80D}' grs:Value='120' 
    grs:Home1Value='120' grs:Home2Value='120' grs:NonRecoverableTax='0' grs:NonRecoverableTaxHome1='0' grs:NonRecoverableTaxHome2='0'>
        <grs:Nominal grs:Coding='SALES.CONF.MARKET' grs:Element1='SALES' grs:Element2='CONF' grs:Element3='MARKET' grs:Element4='' 
        grs:Element5='' grs:Element6='' grs:Element7='' grs:Element8=''></grs:Nominal>
    </grs:NominalPeriod>

</grs:NominalPeriods>

</grs:CommitmentLookup>";
            var actualResult = service.CommitmentReport(NominalsXML, POXML);
            Assert.AreEqual(expectedResult.Replace("'",@""""), actualResult);
        }
    }
}
