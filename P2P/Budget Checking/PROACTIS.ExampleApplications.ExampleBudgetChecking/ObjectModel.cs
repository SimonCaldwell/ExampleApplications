/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleBudgetChecking
{
    internal class ObjectModel
    {
        private const string NS = "http://www.getrealsystems.com/xml/xml-ns";
        internal static Details FromXml(string nominalsXML)
        {

            // Load the xml
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", NS);

            var dom = new XmlDocument(nt);
            dom.LoadXml(nominalsXML);

            // Populate an object model
            var details = new Details();

            var database = (XmlElement)dom.DocumentElement.SelectSingleNode("grs:Database", nsmgr);
            if (database == null) throw new Exception("XML does not contain an element called database.");
            details.DatabaseServer = database.GetAttribute("Server", NS);
            details.Database = database.GetAttribute("DatabaseName", NS);

            var general = (XmlElement)dom.DocumentElement.SelectSingleNode("grs:General", nsmgr);
            if (general == null) throw new Exception("XML does not contain an element called general.");
            details.UserGUID = general.GetAttribute("UserGUID", NS);
            details.CompanyGUID = general.GetAttribute("CompanyGUID", NS);

            var currency = (XmlElement)dom.DocumentElement.SelectSingleNode("grs:Currencies/grs:Currency", nsmgr);
            if (currency == null) throw new Exception("XML does not contain an element called general.");
            details.CurrencyGUID = currency.GetAttribute("CurrencyGUID", NS);
            details.CurrencyType = currency.GetAttribute("Status", NS);
            details.CurrencySymbol = currency.GetAttribute("Symbol", NS);
            details.DecimalPlaces = int.Parse(currency.GetAttribute("DecimalPlaces", NS));

            details.NominalPeriods = new List<NominalPeriod>();
            foreach (XmlElement nomPeriod in dom.DocumentElement.SelectNodes("grs:NominalPeriods/grs:NominalPeriod", nsmgr))
            {
                var nominalPeriod = new NominalPeriod();
                details.NominalPeriods.Add(nominalPeriod);
                nominalPeriod.Year = nomPeriod.GetAttribute("Year", NS);
                nominalPeriod.Period = nomPeriod.GetAttribute("Period", NS);
                nominalPeriod.YearPeriodGUID = nomPeriod.GetAttribute("YearPeriodGUID", NS);
                nominalPeriod.Value = decimal.Parse(nomPeriod.GetAttribute("Value", NS));
                nominalPeriod.Home1Value = decimal.Parse(nomPeriod.GetAttribute("Home1Value", NS));
                nominalPeriod.Home2Value = decimal.Parse(nomPeriod.GetAttribute("Home2Value", NS));
                nominalPeriod.NonRecoverableTax = decimal.Parse(nomPeriod.GetAttribute("NonRecoverableTax", NS));
                nominalPeriod.NonRecoverableTaxHome1 = decimal.Parse(nomPeriod.GetAttribute("NonRecoverableTaxHome1", NS));
                nominalPeriod.NonRecoverableTaxHome2 = decimal.Parse(nomPeriod.GetAttribute("NonRecoverableTaxHome2", NS));

                var nom = (XmlElement)nomPeriod.SelectSingleNode("grs:Nominal", nsmgr);
                if (nom == null) throw new Exception("NominalPeriod element does not contain a nominal element");
                nominalPeriod.Coding = nom.GetAttribute("Coding", NS);
                nominalPeriod.Element1 = nom.GetAttribute("Element1", NS);
                nominalPeriod.Element2 = nom.GetAttribute("Element2", NS);
                nominalPeriod.Element3 = nom.GetAttribute("Element3", NS);
                nominalPeriod.Element4 = nom.GetAttribute("Element4", NS);
                nominalPeriod.Element5 = nom.GetAttribute("Element5", NS);
                nominalPeriod.Element6 = nom.GetAttribute("Element6", NS);
                nominalPeriod.Element7 = nom.GetAttribute("Element7", NS);
                nominalPeriod.Element8 = nom.GetAttribute("Element8", NS);
            }

            return details;
        }
    }

    internal class NominalPeriod
    {
        public decimal Value { get; internal set; }
        public string YearPeriodGUID { get; internal set; }
        public string Period { get; internal set; }
        public string Year { get; internal set; }
        public string Coding { get; internal set; }
        public string Element1 { get; internal set; }
        public string Element2 { get; internal set; }
        public string Element3 { get; internal set; }
        public string Element4 { get; internal set; }
        public string Element5 { get; internal set; }
        public string Element6 { get; internal set; }
        public string Element7 { get; internal set; }
        public string Element8 { get; internal set; }
        public decimal NonRecoverableTaxHome2 { get; internal set; }
        public decimal NonRecoverableTaxHome1 { get; internal set; }
        public decimal NonRecoverableTax { get; internal set; }
        public decimal Home2Value { get; internal set; }
        public decimal Home1Value { get; internal set; }
    }

    internal class Details
    {
        public string DatabaseServer { get; internal set; }
        public string Database { get; internal set; }
        public string UserGUID { get; internal set; }
        public string CompanyGUID { get; internal set; }
        public int DecimalPlaces { get; internal set; }
        public string CurrencySymbol { get; internal set; }
        public string CurrencyType { get; internal set; }
        public string CurrencyGUID { get; internal set; }
        public List<NominalPeriod> NominalPeriods { get; internal set; }
    }
}