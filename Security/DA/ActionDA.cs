using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Security.DA
{
    public sealed class ActionDA : DALBase<Action>
    {
        public ActionDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<DataStructure.Action> GetNotaddedInUser(Guid userId)
        {
            ActionCommandBuilder userCommandBuilder = new ActionCommandBuilder();
            SqlCommand query = userCommandBuilder.GetNotaddedInUser(userId);
            return DBManager.GetCollection<DataStructure.Action>(ConnectionHandler, query);
        }

        public IEnumerable<DataStructure.Action> GetNotaddedInRole(Guid roleId)
        {
            ActionCommandBuilder userCommandBuilder = new ActionCommandBuilder();
            var query = userCommandBuilder.GetNotaddedInRole(roleId);
            return DBManager.GetCollection<DataStructure.Action>(ConnectionHandler, query);
        }
    }
    internal class ActionCommandBuilder
    {
        public SqlCommand GetNotaddedInUser(Guid userId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText =
             " SELECT    Security.[Action].* from Security.[Action] where Security.[Action].Id not in (SELECT        Security.[Action].Id" +
                            " FROM            Security.Action INNER JOIN " +
                            " Security.UserAction ON Security.[Action].Id = Security.UserAction.ActionId  where Security.UserAction.UserId=@User)  order by  Security.[Action].Name ";
            return query;
        }

        public SqlCommand GetNotaddedInRole(Guid roleId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Role", roleId));
            query.CommandText = " SELECT    Security.[Action].* from Security.[Action] where Security.[Action].Id not in (SELECT        Security.[Action].Id" +
                            " FROM            Security.[Action] INNER JOIN " +
                            " Security.RoleAction ON Security.[Action].Id = Security.RoleAction.ActionId  where Security.RoleAction.RoleId=@Role)  order by  Security.[Action].Name ";
            return query;
        }
    }
}
