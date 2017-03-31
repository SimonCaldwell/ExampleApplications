using System;
using System.Xml;

namespace PROACTIS.ExampleApplications.ExampleBudgetChecking
{
    internal class Report
    {
        private const string NS = "http://www.getrealsystems.com/xml/xml-ns";
        private readonly XmlDocument dom;
        private readonly XmlNamespaceManager nsmgr;
        private readonly XmlElement headings;
        private readonly XmlElement items;

        public Report()
        {
            // Create a new DOM
            var nt = new NameTable();
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("grs", NS);
            this.dom = new XmlDocument(nt);

            this.dom.AppendChild(this.dom.CreateElement("grs:HeadedList", NS));

            this.headings = this.dom.CreateElement("grs:Headings", NS);
            this.dom.DocumentElement.AppendChild(this.headings);

            this.items = this.dom.CreateElement("grs:Items", NS);
            this.dom.DocumentElement.AppendChild(this.items);
        }


        internal void AddColumn(string caption)
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Number", NS, (this.headings.ChildNodes.Count + 1).ToString());
            column.SetAttribute("Type", NS, "");
            column.SetAttribute("BudgetType", NS, "");
            column.InnerText = caption;

            this.headings.AppendChild(column);
        }

        internal void AddCurrencyBudgetColumn(string caption)
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Number", NS, (this.headings.ChildNodes.Count + 1).ToString());
            column.SetAttribute("Type", NS, "Currency");
            column.SetAttribute("BudgetType", NS, "Budget");
            column.InnerText = caption;

            this.headings.AppendChild(column);
        }

        internal void AddCurrencyCostColumn(string caption)
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Number", NS, (this.headings.ChildNodes.Count + 1).ToString());
            column.SetAttribute("Type", NS, "Currency");
            column.SetAttribute("BudgetType", NS, "Cost");
            column.InnerText = caption;

            this.headings.AppendChild(column);
        }

        internal void AddHighlightColumn()
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Number", NS, (this.headings.ChildNodes.Count + 1).ToString());
            column.SetAttribute("Type", NS, "Highlight");
            column.InnerText = "Highlight";

            this.headings.AppendChild(column);
        }

        internal void AddLine(params XmlElement[] columns)
        {
            var item = this.dom.CreateElement("grs:Item", NS);
            this.items.AppendChild(item);

            var number = 0;
            foreach (var column in columns)
            {
                number++;
                column.SetAttribute("Number", NS, number.ToString());
                item.AppendChild(column);
            }
        }

        internal XmlElement CreateStandardColumn(string value)
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Type", NS, "");
            column.InnerText = value;

            return column;
        }


        internal XmlElement CreateCurrencyColumn(decimal value, string currencySymbol, int decimalPlaces, string hyperLink = "")
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Type", NS, "Currency");
            column.SetAttribute("CurrencySymbol", NS, currencySymbol);
            column.SetAttribute("DecimalPlaces", NS, decimalPlaces.ToString());

            if (!string.IsNullOrWhiteSpace(hyperLink))
                column.SetAttribute("HyperLink", NS, hyperLink);

            column.InnerText = value.ToString();

            return column;
        }

        internal XmlElement CreateHighlightColumn(bool value)
        {
            var column = this.dom.CreateElement("grs:Column", NS);
            column.SetAttribute("Type", NS, "Highlight");
            column.InnerText = value.ToString();

            return column;
        }

        internal string ToXML()
        {
            return dom.OuterXml;
        }
    }
}