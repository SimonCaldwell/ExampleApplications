using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using PROACTIS.P2P.grsCustInterfaces;

namespace PROACTIS.ExampleApplication.TransactionalCommitments
{
    /// <summary>
    /// Example showing how transaction handling works within a commitment posting DLL
    /// </summary>
    public class Services : PROACTIS.P2P.grsCustInterfaces.ICommitmentProcessor
    {
        void ICommitmentProcessor.ProcessCommitment(Guid commitmentGUID, string commitmentXML, string database, string databaseServer)
        {
            // Generate a connection string back to the main p2p database
            var sb = new SqlConnectionStringBuilder();
            sb.DataSource = databaseServer;
            sb.InitialCatalog = database;
            sb.IntegratedSecurity = true;
            var cs = sb.ToString();

            // Use the following SQL to generate the Test table in your database
            //  CREATE TABLE dbo.Test
            //  (CommitmentGUID UNIQUEIDENTIFIER NOT NULL)


            // Case 1 - Normal case.  Your code is part of the parent transaction and so your insert will be committed
            // once the call completed.
            using (var cn = new SqlConnection(cs))
            {
                cn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "insert into dbo.test (CommitmentGUID) select @CommitmentGUID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CommitmentGUID", commitmentGUID);
                    cmd.ExecuteNonQuery();

                    //We are in a transaction,  so nothing has been committed yet
                }
            }
            //Everything has worked,  so our caller will commit the insert into dbo.Test


            // Case 2 - Error case.  Your code is part of the parent transaction and so your insert will be rolled
            // back as your code has thrown an exception.
            //using (var cn = new SqlConnection(cs))
            //{
            //    cn.Open();

            //    using (var cmd = new SqlCommand())
            //    {
            //        cmd.Connection = cn;
            //        cmd.CommandText = "insert into dbo.test (CommitmentGUID) select @CommitmentGUID";
            //        cmd.CommandType = CommandType.Text;
            //        cmd.Parameters.AddWithValue("@CommitmentGUID", commitmentGUID);
            //        cmd.ExecuteNonQuery();

            //        //We are in a transaction,  so nothing has been committed yet
            //    }
            //}
            //// Throw an error,  which will cause a our caller to rollback our insert into dbo.Test
            //throw new Exception("This is an error");


            // Case 3 - Advanced Case.  Your code is not part of the parent transaction so your insert will be
            // committed even though your code raised an exception.
            //using (var tx = new TransactionScope(TransactionScopeOption.Suppress))
            //{
            //    using (var cn = new SqlConnection(cs))
            //    {
            //        cn.Open();

            //        using (var cmd = new SqlCommand())
            //        {
            //            cmd.Connection = cn;
            //            cmd.CommandText = "insert into dbo.test (CommitmentGUID) select @CommitmentGUID";
            //            cmd.CommandType = CommandType.Text;
            //            cmd.Parameters.AddWithValue("@CommitmentGUID", commitmentGUID);
            //            cmd.ExecuteNonQuery();

            //            // Note,  we aren't in a transaction,  so this is committed at this point
            //        }
            //    }

            //    // Throw an error,  but the insert into dbo.Test won't be rolled back
            //    throw new Exception("This is an error");
            //}
        }


    }
}
