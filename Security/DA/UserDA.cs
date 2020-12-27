using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Security.DataStructure;
using Radyn.Security.Tools;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Radyn.Security.DA
{

    public sealed class UserDA : DALBase<User>
    {
        public UserDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public Access AccessMenu(Guid userId, string url)
        {
            UserCommandBuilder userCommandBuilder = new UserCommandBuilder();
            SqlCommand query = userCommandBuilder.AccessMenu(userId, url);
            return DBManager.GetCollection<Access>(ConnectionHandler, query).FirstOrDefault();
        }
    }
    internal class UserCommandBuilder
    {
        public SqlCommand AccessMenu(Guid userId, string url)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@User", userId));
            query.Parameters.Add(new SqlParameter("@url", url));
            query.CommandText =
            " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 " Security.RoleMenu ON Security.Menu.Id = Security.RoleMenu.MenuId INNER JOIN " +
                                 " Security.GroupRole ON Security.RoleMenu.RoleId = Security.GroupRole.RoleId LEFT OUTER JOIN " +
                                 " Security.UserGroup ON Security.GroupRole.GroupId = Security.UserGroup.GroupId where Security.Menu.Enabled=1 and  Security.UserGroup.UserId=@User and LOWER( Security.Menu.Url)=@url   " +
                                 "union" +
                                 " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId  " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 " Security.UserMenu ON Security.Menu.Id = Security.UserMenu.MenuId where Security.Menu.Enabled=1 and Security.UserMenu.UserId=@User and LOWER( Security.Menu.Url)=@url " +
                                 " union" +
                                 " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 " Security.OperationMenu ON Security.Menu.Id = Security.OperationMenu.MenuId INNER JOIN " +
                                 " Security.RoleOperation ON Security.OperationMenu.OperationId = Security.RoleOperation.OperationId INNER JOIN " +
                                 " Security.UserRole ON Security.RoleOperation.RoleId = Security.UserRole.RoleId where Security.Menu.Enabled=1 and Security.UserRole.UserId=@User and LOWER( Security.Menu.Url)=@url " +
                                 " union" +
                                 " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId  " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 " Security.OperationMenu ON Security.Menu.Id = Security.OperationMenu.MenuId INNER JOIN " +
                                 " Security.UserOperation ON Security.OperationMenu.OperationId = Security.UserOperation.OperationId where Security.Menu.Enabled=1 and Security.UserOperation.UserId=@User and LOWER( Security.Menu.Url)=@url " +
                                 " union" +
                                 " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 "Security.RoleMenu ON Security.Menu.Id = Security.RoleMenu.MenuId INNER JOIN " +
                                 " Security.GroupRole ON Security.RoleMenu.RoleId = Security.GroupRole.RoleId INNER JOIN " +
                                 "Security.UserGroup ON Security.GroupRole.GroupId = Security.UserGroup.GroupId where Security.Menu.Enabled=1 and Security.UserGroup.UserId=@User and LOWER( Security.Menu.Url)=@url " +
                                 " union" +
                                 " SELECT DISTINCT " +
                                 " Security.Menu.Id, Security.Menu.Title, Security.Menu.HelpId  " +
                                 " FROM         Security.Menu INNER JOIN " +
                                 " Security.RoleMenu ON Security.Menu.Id = Security.RoleMenu.MenuId INNER JOIN " +
                                 " Security.UserRole ON Security.RoleMenu.RoleId = Security.UserRole.RoleId where Security.Menu.Enabled=1 and Security.UserRole.UserId=@User and LOWER( Security.Menu.Url)=@url ";
            return query;
        }



    }
}
