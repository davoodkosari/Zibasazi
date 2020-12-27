using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.Security.DA
{
    public sealed class OperationMenuDA : DALBase<OperationMenu>
    {
        public OperationMenuDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public List<Menu> GetOprationMenu(Guid oprationId)
        {
            OperationMenuCommandBuilder operationMenuCommandBuilder = new OperationMenuCommandBuilder();
            SqlCommand query = operationMenuCommandBuilder.GetOprationMenu(oprationId);
            return DBManager.GetCollection<Menu>(base.ConnectionHandler, query);
        }
        public List<Menu> GetOprationMenu(Guid oprationId, int groupId)
        {
            OperationMenuCommandBuilder operationMenuCommandBuilder = new OperationMenuCommandBuilder();
            var query = operationMenuCommandBuilder.GetOprationMenu(oprationId, groupId);
            return DBManager.GetCollection<Menu>(base.ConnectionHandler, query);
        }


        public IEnumerable<Menu> GetMenuTree(Guid oprationId)
        {
            OperationMenuCommandBuilder operationMenuCommandBuilder = new OperationMenuCommandBuilder();
            var query = operationMenuCommandBuilder.GetMenuTree(oprationId);
            return DBManager.GetCollection<Menu>(base.ConnectionHandler, query);
        }


    }
    internal class OperationMenuCommandBuilder
    {
        public SqlCommand GetOprationMenu(Guid oprationId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@OP", oprationId));
            query.CommandText =
                "SELECT     [Security].Menu.* " +
                " FROM         [Security].Menu INNER JOIN " +
                " [Security].OperationMenu ON [Security].Menu.Id = [Security].OperationMenu.MenuId " +
                " where [Security].OperationMenu.OperationId=@OP and [Security].Menu.ParentId is null  and   [Security].Menu.Display=1 and [Security].Menu.[Enabled]=1 order by [Security].Menu.[Order]";
            return query;
        }
        public SqlCommand GetOprationMenu(Guid oprationId, int groupId)
        {
            var query=new SqlCommand();
            query.Parameters.Add(new SqlParameter("@OP", oprationId));
            query.Parameters.Add(new SqlParameter("@group", groupId));
            query.CommandText =
                "SELECT     [Security].Menu.* " +
                " FROM         [Security].Menu INNER JOIN " +
                " [Security].OperationMenu ON [Security].Menu.Id = [Security].OperationMenu.MenuId " +
                " where [Security].OperationMenu.OperationId=@OP and [Security].Menu.MenuGroupId=@group and [Security].Menu.ParentId is null  and   [Security].Menu.Display=1 and [Security].Menu.[Enabled]=1 order by [Security].Menu.[Order]";
            return query;
        }


        public SqlCommand GetMenuTree(Guid oprationId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@OP", oprationId));
            query.CommandText =
                "SELECT     [Security].Menu.* " +
                " FROM         [Security].Menu INNER JOIN " +
                " [Security].OperationMenu ON [Security].Menu.Id = [Security].OperationMenu.MenuId " +
                " where [Security].OperationMenu.OperationId=@OP and [Security].Menu.ParentId is null and   [Security].Menu.[Enabled]=1 order by [Security].Menu.[Order]";
            return query;

        }


    }
}
