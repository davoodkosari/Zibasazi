using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Radyn.ContentManager.DA
{
    public sealed class MenuDA : DALBase<Menu>
    {
        public MenuDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public IEnumerable<Menu> GetParents(Guid? congressId)
        {
            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            var query = menuCommandBuilder.GetParents(congressId);
            return DBManager.GetCollection<Menu>(base.ConnectionHandler, query);
        }

        public IEnumerable<Menu> GetParentsAndChilds(Guid? congressId)
        {
            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            string query = menuCommandBuilder.GetParentsChilds(congressId);
            return DBManager.GetCollection<Menu>(base.ConnectionHandler, query);
        }


        public int GetMaxOrder()
        {

            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            string query = menuCommandBuilder.GetMaxOrder();
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }

        public IEnumerable<Menu> GetChildMenu(Guid parentMenuId)
        {
            MenuCommandBuilder menuCommandBuilder = new MenuCommandBuilder();
            SqlCommand query = menuCommandBuilder.GetChildMenu(parentMenuId);
            return DBManager.GetCollection<Menu>(ConnectionHandler, query);
        }
    }
    internal class MenuCommandBuilder
    {
        public string GetParentsChilds(Guid? congressId)
        {
            if (congressId == null || congressId == Guid.Empty)
            {
                return string.Format("SELECT * FROM [ContentManage].[Menu] ORDER BY [ORDER]");
            }
            return string.Format("SELECT         ContentManage.Menu.* " +
                        " FROM            ContentManage.Menu INNER JOIN " +
                         " Congress.CongressMenu ON ContentManage.Menu.Id = Congress.CongressMenu.MenuId " +
                          " WHERE Congress.CongressMenu.CongressId = '{0}';", congressId);

        }

        public SqlCommand GetParents(Guid? congressId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@congressId", congressId));
            if (congressId == null || congressId == Guid.Empty)
            {
                query.CommandText = "SELECT * FROM [ContentManage].[Menu] WHERE [ParentId] is null  ORDER BY [ORDER]";
            }
            query.CommandText = "SELECT         ContentManage.Menu.* " +
                        " FROM            ContentManage.Menu INNER JOIN " +
                         " Congress.CongressMenu ON ContentManage.Menu.Id = Congress.CongressMenu.MenuId " +
                          " WHERE ContentManage.Menu.[ParentId] is null And Congress.CongressMenu.CongressId = @congressId";
            return query;
        }
        public string GetMaxOrder()
        {
            return string.Format("SELECT Max([ORDER]) FROM [ContentManage].[Menu] WHERE  IsExternal=0  ");
        }

        public SqlCommand GetChildMenu(Guid parentMenuId)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@parentMenuId", parentMenuId));
            query.CommandText = "SELECT * FROM [ContentManage].[Menu] WHERE  [ParentId]=@parentMenuId ORDER BY [ORDER]";
            return query;
        }
    }
}
