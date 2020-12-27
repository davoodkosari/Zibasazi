using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Help.DA
{
    public sealed class HelpDA : DALBase<Help.DataStructure.Help>
    {
        public HelpDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }

        public IEnumerable<DataStructure.Help> Search(string txt)
        {
            HelpCommandBuilder helpCommandBuilder = new HelpCommandBuilder();
            SqlCommand query = helpCommandBuilder.Search(txt);
            return DBManager.GetCollection<DataStructure.Help>(base.ConnectionHandler, query);
        }
    }
    internal class HelpCommandBuilder
    {
        public SqlCommand Search(string txt)
        {
            SqlCommand query = new SqlCommand();
            string where = "";

            if (!string.IsNullOrEmpty(txt))
            {
                query.Parameters.Add(new SqlParameter("@Text", txt));
                where += "Help.DefaultTitle like N'%@Text%' or ";
                where += "Help.DefaultConent like N'%@Text%' or ";
                where += "HelpContent.Content like N'%@Text%' or ";
                where += "HelpContent.Title like N'%@Text%' or ";
            }
            if (!string.IsNullOrEmpty(where))
            {
                where = where.Substring(0, where.Length - 3);
                query.CommandText = string.Format("{0} where {1}  order by HelpContent.CreateDate desc ", query, where);
            }
            return query;
        }
    }
}
