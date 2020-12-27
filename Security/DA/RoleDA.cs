using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Security.DA
{
    public sealed class RoleDA : DALBase<Role>
    {
        public RoleDA(IConnectionHandler connectionHandler) : base(connectionHandler)
        { }

        public IEnumerable<Role> GetNotAddedInUser(Guid userId)
        {
            RoleCommandBuilder userCommandBuilder = new RoleCommandBuilder();
            SqlCommand query = userCommandBuilder.GetNotAddedInUser(userId);
            return DBManager.GetCollection<Role>(ConnectionHandler, query);
        }

        public IEnumerable<Role> GetNotAddedInGroup(Guid groupId)
        {
            RoleCommandBuilder userCommandBuilder = new RoleCommandBuilder();
            SqlCommand query = userCommandBuilder.GetNotAddedInGroup(groupId);
            return DBManager.GetCollection<Role>(ConnectionHandler, query);
        }
    }
    internal class RoleCommandBuilder
    {
        public SqlCommand GetNotAddedInUser(Guid userId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText =
            " SELECT    Security.Role.* from Security.Role where Security.Role.Id not in (SELECT        Security.Role.Id" +
                                 " FROM            Security.Role INNER JOIN " +
                                 " Security.UserRole ON Security.Role.Id = Security.UserRole.RoleId  where Security.UserRole.UserId=@User)";
            return query;
        }

        public SqlCommand GetNotAddedInGroup(Guid groupId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@group", groupId));
            query.CommandText =
            " SELECT    Security.Role.* from Security.Role where Security.Role.Id not in (SELECT        Security.Role.Id " +
                                  " FROM            Security.Role INNER JOIN " +
                                  " Security.GroupRole ON Security.Role.Id = Security.GroupRole.RoleId  where Security.GroupRole.GroupId=@group)";
            return query;
        }
    }
}
