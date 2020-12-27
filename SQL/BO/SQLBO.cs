using System.Data;
using System.Data.SqlClient;
using Radyn.SQL.Enums;

namespace Radyn.SQL.BO
{
    internal class SQLBO
    {
        public object ExecuteCommand(string connection, string commend, ref SqlOutType sqlOutType)
        {

            var sqlconection = new SqlConnection(connection);
            sqlconection.Open();
            commend = commend.ToLower();
            if (commend.Contains("select"))
            {
                var ScReader = new SqlDataAdapter(commend, sqlconection);
                var dataSet = new DataTable();
                ScReader.Fill(dataSet);
                sqlOutType = SqlOutType.Datatable;
                return dataSet;
            }

            var ScCommand = new SqlCommand(commend, sqlconection);
            var executeNonQuery = ScCommand.ExecuteNonQuery();
            sqlOutType = SqlOutType.Object;
            sqlconection.Close();
            return executeNonQuery;
        }
    }
}
