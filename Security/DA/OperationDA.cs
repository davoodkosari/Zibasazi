using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Radyn.Security.DA
{
    public sealed class OperationDA : DALBase<Operation>
    {
        public OperationDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public List<Operation> GetAllByUserId(Guid userId)
        {
            OperationCommandBuilder userCommandBuilder = new OperationCommandBuilder();
            SqlCommand query = userCommandBuilder.GetAllByUserId(userId);
            return DBManager.GetCollection<Operation>(ConnectionHandler, query);
        }
        public async Task<List<Operation>> GetAllByUserIdAsync(Guid userId)
        {
            OperationCommandBuilder userCommandBuilder = new OperationCommandBuilder();
            SqlCommand query = userCommandBuilder.GetAllByUserId(userId);
            return await DBManager.GetCollectionAsync<Operation>(ConnectionHandler, query);
        }
        public IEnumerable<Operation> GetNotAddedInRole(Guid roleId)
        {
            OperationCommandBuilder userCommandBuilder = new OperationCommandBuilder();
            SqlCommand query = userCommandBuilder.GetNotAddedInRole(roleId);
            return DBManager.GetCollection<Operation>(ConnectionHandler, query);
        }

        public IEnumerable<Operation> GetNotAddedInUser(Guid userId)
        {
            OperationCommandBuilder userCommandBuilder = new OperationCommandBuilder();
            var query = userCommandBuilder.GetNotAddedInUser(userId);
            return DBManager.GetCollection<Operation>(ConnectionHandler, query);
        }
    }
    internal class OperationCommandBuilder
    {
        public SqlCommand GetAllByUserId(Guid userId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText = "SELECT DISTINCT  " +
                                 " Security.Operation.* " +
                                 " FROM         Security.Operation INNER JOIN " +
                                 " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN  " +
                                 " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId where Security.Operation.Enabled=1 and  Security.UserRole.UserId=@User " +
                                 " union " +
                                 "SELECT   DISTINCT  Security.Operation.* " +
                                 " FROM         Security.Operation INNER JOIN " +
                                 " Security.UserOperation ON Security.Operation.Id = Security.UserOperation.OperationId where Security.Operation.Enabled=1 and Security.UserOperation.UserId=@User " +
                                 "union " +
                                 " SELECT   DISTINCT  Security.Operation.* " +
                                 " FROM         Security.Operation INNER JOIN " +
                                 " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN " +
                                 " Security.GroupRole ON Security.RoleOperation.RoleId = Security.GroupRole.RoleId INNER JOIN " +
                                 " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId where  Security.Operation.Enabled=1 and Security.UserRole.UserId=@User ";
            return query;
        }

        public SqlCommand GetNotAddedInRole(Guid roleId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@Role", roleId));
            query.CommandText =
                                " select * from  Security.Operation where  Security.Operation.Id not in ( SELECT     distinct   Security.Operation.Id " +
                                " FROM            Security.Operation INNER JOIN " +
                                " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId where Security.RoleOperation.RoleId=@Role)";
            return query;

        }

        public SqlCommand GetNotAddedInUser(Guid userId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText =
            " select * from  Security.Operation where  Security.Operation.Id not in ( SELECT     distinct   Security.Operation.Id " +
                   " FROM            Security.Operation INNER JOIN " +
                   " Security.UserOperation ON Security.Operation.Id = Security.UserOperation.OperationId where Security.UserOperation.UserId=@User)";
            return query;
        }
    }
}
