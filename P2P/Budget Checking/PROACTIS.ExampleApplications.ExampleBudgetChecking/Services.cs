/*
 * This file is subject to the terms and conditions defined in file 'https://github.com/proactis-documentation/ExampleApplications/LICENSE.txt'
 */
using System;

namespace PROACTIS.ExampleApplications.ExampleBudgetChecking
{
    public class Services : P2P.grsCustInterfaces.ICustCommit, P2P.grsCustInterfaces.IOverSpend
    {
        /// <summary>
        /// Return True only if none of the lines on this order have exceeded their budgets.
        /// </summary>
        /// <param name="NominalsXML"></param>
        /// <param name="POXML"></param>
        /// <returns></returns>
        public bool CommitmentCheck(string NominalsXML, string POXML)
        {
            // Convert the supplied nominals xml into a object model
            var details = ObjectModel.FromXml(NominalsXML);

            // Check each nominal in turn
            foreach (var nominal in details.NominalPeriods)
            {
                /*******************************
                 * 
                 *     Place your code here
                 * 
                 ******************************/
                if (nominal.Value > 100)
                {
                    // Exit as soon as we find a failure.
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// Generates a report showing which lines are exceeding their budgets and why
        /// </summary>
        /// <param name="NominalsXML"></param>
        /// <param name="POXML"></param>
        /// <returns></returns>
        public string CommitmentReport(string NominalsXML, string POXML)
        {
            // Convert the supplied nominals xml into a object model
            var details = ObjectModel.FromXml(NominalsXML);

            // Define the columns for our report
            var report = new Report();
            report.AddColumn("Nominal Coding");
            report.AddCurrencyBudgetColumn("Budget For Year");
            report.AddCurrencyCostColumn("Spend To Date");
            report.AddCurrencyCostColumn("Accruals");
            report.AddCurrencyCostColumn("This Document");
            report.AddCurrencyCostColumn("Remaining Budget");
            report.AddHighlightColumn();

            // Add the lines onto the report
            foreach (var line in details.NominalPeriods)
            {
                var BudgetForYear = 10000M;
                var SpendToDate = 6000M;
                var Accruals = 1000M;
                var Remaining = BudgetForYear - SpendToDate - Accruals - line.Home1Value;

                report.AddLine(
                                report.CreateStandardColumn(line.Coding),
                                report.CreateCurrencyColumn(BudgetForYear, details.CurrencySymbol, details.DecimalPlaces),
                                report.CreateCurrencyColumn(SpendToDate, details.CurrencySymbol, details.DecimalPlaces),  
                                report.CreateCurrencyColumn(Accruals, details.CurrencySymbol, details.DecimalPlaces),  
                                report.CreateCurrencyColumn(line.Home1Value, details.CurrencySymbol, details.DecimalPlaces),  
                                report.CreateCurrencyColumn(Remaining, details.CurrencySymbol, details.DecimalPlaces, "http://www.proactis.com"),  
                                report.CreateHighlightColumn(Remaining < 0)
                              );
            }

            // Return the report as XML
            return report.ToXML();
        }

        /// <summary>
        /// Calculate by how much all the lines exceed the budget in total
        /// </summary>
        /// <param name="NominalsXML"></param>
        /// <param name="POXML"></param>
        /// <returns></returns>
        public decimal GetOverspend(string NominalsXML, string POXML)
        {
            // Convert the supplied nominals xml into a object model
            var details = ObjectModel.FromXml(NominalsXML);

            // Keep a running total
            var totalOverSpend = 0M;

            // Check each nominal in turn
            foreach (var nominal in details.NominalPeriods)
            {
                /*******************************
                 * 
                 *     Place your code here
                 * 
                 ******************************/
                if (nominal.Value > 100)
                {
                    // We are pretending that we only have £100 available,  so anything over that
                    // is classed as an overspend.
                    totalOverSpend += (nominal.Value - 100);
                }
            }


            return totalOverSpend;
        }
    }
}
