using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Congress.DA
{
    public sealed class UserDA : DALBase<User>
    {
        public UserDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }



        public IEnumerable<User> GetSimilarUser(Guid congressId, string name, string lastname)
        {
            UserCommandBuilder homaCommandBuilder = new UserCommandBuilder();
            SqlCommand query = homaCommandBuilder.GetSimilarUser(congressId, name, lastname);
            return DBManager.GetCollection<User>(ConnectionHandler, query);
        }

    }
    internal class UserCommandBuilder
    {


        public SqlCommand GetSimilarUser(Guid congressId, string name, string lastname)
        {
            SqlCommand q = new SqlCommand();
            q.Parameters.Add(new SqlParameter("@congressId", congressId));
            q.Parameters.Add(new SqlParameter("@name", name.Trim()));
            q.Parameters.Add(new SqlParameter("@lastname", lastname.Trim()));
            string query =
                string.Format(
                    "  select   Congress.[User].Id  from  Congress.[User] INNER JOIN " +
                    " EnterpriseNode.EnterpriseNode ON Congress.[User].Id = EnterpriseNode.EnterpriseNode.Id inner JOIN " +
                    " EnterpriseNode.RealEnterpriseNode ON EnterpriseNode.EnterpriseNode.Id = EnterpriseNode.RealEnterpriseNode.Id");
            string where = " Congress.[User].CongressId=@congressId and ";
            where += " EnterpriseNode.RealEnterpriseNode.FirstName=@name and ";
            where += " EnterpriseNode.RealEnterpriseNode.LastName=@lastname and ";
            string substring = where.Substring(0, where.Length - 4);
            q.CommandText = string.Format(" {0} where {1} order by Congress.[User].RegisterDate desc  ", query, substring);
            return q;
        }



    }
}
