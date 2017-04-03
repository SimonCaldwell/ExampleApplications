using PROACTIS.ExampleApplications.CustomTabMVC.Models;
using System;
using System.Threading.Tasks;

namespace PROACTIS.ExampleApplications.CustomTabMVC
{
    public class Database : Proactis.Common.P2PDatabaseBase
    {
        public Database(string databaseTitle) : base(databaseTitle, "Custom Tab") { }

        internal async Task<SessionDetails> GetSessionDetailsFromTokenAsync(string token)
        {
            using (var cmd = this.CreateNewSqlCommandForProcedure("DSDBA.usp_cust_GetSessionDetailsFromToken"))
            {
                cmd.Parameters.AddWithValue("@Token", token);

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (!await dr.ReadAsync())
                        throw new Exception("No session exists - no row returned");

                    if (await dr.IsDBNullAsync(dr.GetOrdinal("UserGUID")))
                        throw new Exception("No session exists - no data returned");

                    return new SessionDetails()
                    {
                        CompanyCode = (string)dr["CompanyCode"] ?? "",
                        CompanyGUID = (Guid)dr["CompanyGUID"],
                        DepartmentCode = (string)dr["DepartmentCode"] ?? "",
                        DepartmentGUID = (Guid)dr["DepartmentGUID"],
                        Expires = (DateTime)dr["Expires"],
                        LoginID = (string)dr["LoginID"] ?? "",
                        SessionID = (string)dr["SessionID"] ?? "",
                        StoreCode = (string)dr["StoreCode"] ?? "",
                        StoreGUID = (Guid)dr["StoreGUID"],
                        UserGUID = (Guid)dr["UserGUID"],
                    };

                }
            }
        }
    }
}