using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Security.DA
{
    public sealed class GroupDA : DALBase<Group>
    {
        public GroupDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }

        public IEnumerable<Group> GetNotAddedInUser(Guid userId)
        {
            GroupCommandBuilder userCommandBuilder = new GroupCommandBuilder();
            var query = userCommandBuilder.GetNotAddedInUser(userId);
            return DBManager.GetCollection<Group>(ConnectionHandler, query);
        }
    }
    internal class GroupCommandBuilder
    {
        public SqlCommand GetNotAddedInUser(Guid userId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText =
                " SELECT    Security.[Group].* from Security.[Group] where Security.[Group].Id not in (SELECT        Security.[Group].Id" +
                " FROM            Security.[Group] INNER JOIN " +
                " Security.UserGroup ON Security.[Group].Id = Security.UserGroup.[GroupId]  where Security.UserGroup.UserId=@User)  order by  Security.[Group].Name";
            return query;
        }
    }
}
