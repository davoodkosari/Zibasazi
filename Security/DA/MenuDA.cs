using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Radyn.Security.DA
{
    public sealed class MenuDA : DALBase<Menu>
    {
        public MenuDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
        public List<Menu> GetChildMenus(List<Guid> parentMenuId, Guid userId, bool? display = true)
        {
            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            SqlCommand query = menuCommandBuilder.GetChildMenus(parentMenuId, userId);
            return DBManager.GetCollection<Menu>(ConnectionHandler, query);
        }
        public List<Menu> GetChildMenu(Guid parentMenuId, Guid? userId, bool? display = true)
        {
            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            var query = menuCommandBuilder.GetChildMenu(parentMenuId, userId);
            return DBManager.GetCollection<Menu>(ConnectionHandler, query);
        }




    }
    internal class MenuCommandBuilder
    {

        public SqlCommand GetChildMenus(List<Guid> parentMenuId, Guid userId, bool? display = true)
        {
            SqlCommand query = new SqlCommand();

            string rolesChecked = parentMenuId.Aggregate("", (current, role1) => current + ("'" + role1 + "'" + ","));
            rolesChecked = rolesChecked.Substring(0, rolesChecked.Length - 1);
            string args = display != null ? string.Format(" and   [Security].Menu.Display={0}", (bool)display ? "1" : "0") : "";
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.CommandText =
                string.Format("SELECT    DISTINCT  Security.Menu.* FROM         Security.Menu INNER JOIN " +
                              " Security.OperationMenu ON Security.Menu.Id = Security.OperationMenu.MenuId where ParentId in ({0}) {1} and [Security].Menu.[Enabled]=1  and  Security.OperationMenu.OperationId in ( " +
                              " SELECT DISTINCT   Security.Operation.Id  " +
                              " FROM         Security.Operation INNER JOIN  " +
                              " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN   " +
                              " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId " +
                              " union" +
                              " SELECT   DISTINCT  Security.Operation.Id  " +
                              " FROM         Security.Operation INNER JOIN  " +
                              " Security.UserOperation ON Security.Operation.Id = Security.UserOperation.OperationId " +
                              " union" +
                              " SELECT   DISTINCT  Security.Operation.Id  " +
                              " FROM         Security.Operation INNER JOIN  " +
                              " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN  " +
                              " Security.GroupRole ON Security.RoleOperation.RoleId = Security.GroupRole.RoleId INNER JOIN  " +
                              " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId where Security.UserRole.UserId=@User ) order by [Security].Menu.[Order] ",
                    rolesChecked, args);
            return query;

        }

        public SqlCommand GetChildMenu(Guid parentMenuId, Guid? userId, bool? display = true)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            string args = display != null ? string.Format(" and   [Security].Menu.Display={0}", (bool)display ? "1" : "0") : "";
            query.CommandText = userId.HasValue
                ? string.Format("SELECT    DISTINCT  Security.Menu.* FROM         Security.Menu INNER JOIN " +
                                " Security.OperationMenu ON Security.Menu.Id = Security.OperationMenu.MenuId where ParentId='{0}' {1} and [Security].Menu.[Enabled]=1  and  Security.OperationMenu.OperationId in ( " +
                                " SELECT DISTINCT   Security.Operation.Id  " +
                                " FROM         Security.Operation INNER JOIN  " +
                                " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN   " +
                                " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId " +
                                " union" +
                                " SELECT   DISTINCT  Security.Operation.Id  " +
                                " FROM         Security.Operation INNER JOIN  " +
                                " Security.UserOperation ON Security.Operation.Id = Security.UserOperation.OperationId " +
                                " union" +
                                " SELECT   DISTINCT  Security.Operation.Id  " +
                                " FROM         Security.Operation INNER JOIN  " +
                                " Security.RoleOperation ON Security.Operation.Id = Security.RoleOperation.OperationId INNER JOIN  " +
                                " Security.GroupRole ON Security.RoleOperation.RoleId = Security.GroupRole.RoleId INNER JOIN  " +
                                " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId where Security.UserRole.UserId=@User ) order by [Security].Menu.[Order] ",
                                parentMenuId, args)
                : string.Format("SELECT     [Security].Menu.* " +
                                " FROM         [Security].Menu " +
                                " where  [Security].Menu.ParentId='{0}' {1} and [Security].Menu.[Enabled]=1 order by [Security].Menu.[Order]",
                    parentMenuId, args);
            return query;
        }



    }
}
