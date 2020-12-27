using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Radyn.FileManager.DAL
{
    public sealed class FolderDA : DALBase<Folder>
    {
        public FolderDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {

        }
        public IEnumerable<Folder> GetAll(bool isexternal)
        {

            FolderCommandBuilder commandBiulder = new FolderCommandBuilder();
            var command = commandBiulder.GetAll(isexternal);
            return DBManager.GetCollection<Folder>(base.ConnectionHandler, command);
        }
        public IEnumerable<Folder> GetParents(bool isexternal)
        {

            FolderCommandBuilder commandBiulder = new FolderCommandBuilder();
            var command = commandBiulder.GetParents(isexternal);
            return DBManager.GetCollection<Folder>(base.ConnectionHandler, command);
        }
        public Folder GetFirstParent(bool isexternal)
        {

            FolderCommandBuilder commandBiulder = new FolderCommandBuilder();
            SqlCommand command = commandBiulder.GetFirstParent(isexternal);
            return DBManager.GetObject<Folder>(base.ConnectionHandler, command);
        }


    }
    internal class FolderCommandBuilder
    {
        public SqlCommand GetAll(bool isexternal)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@External", isexternal ? "1" : "0"));
            query.CommandText = "SELECT * FROM [FileManager].[Folder] WHERE IsExternal=@External ORDER BY [Title]";
            return query;
        }
        public SqlCommand GetParents(bool isexternal)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@External", isexternal ? "1" : "0"));
            query.CommandText = "SELECT * FROM [FileManager].[Folder] WHERE [ParentFolderId] is null and IsExternal=@External ORDER BY [Title]";
            return query;
        }
        public SqlCommand GetFirstParent(bool isexternal)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@External", isexternal ? "1" : "0"));
            query.CommandText =
            "SELECT Top(1)* FROM [FileManager].[Folder] WHERE [ParentFolderId] is null and IsExternal=@External ORDER BY [Title]";
            return query;
        }


    }
}
