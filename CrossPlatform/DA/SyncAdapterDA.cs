using System;
using System.Collections.Generic;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.DA
{
    public sealed class SyncAdapterDA : DALBase<SyncAdapter>
    {
        public SyncAdapterDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public int Count(string tableName, string sourceId)
        {
            var syncAdapterCommandBuilder = new SyncAdapterCommandBuilder();
            var query = syncAdapterCommandBuilder.Exist(tableName, sourceId);
            var count = (int)DBManager.ExecuteScalar(this.ConnectionHandler, query);
            return count;
        }

        public List<SyncAdapter> GetPublicNewVersion(string tableName, long versionId)
        {
            var syncAdapterCommandBuilder = new SyncAdapterCommandBuilder();
            var query = syncAdapterCommandBuilder.GetPublicNewVersion(tableName, versionId);
            return DBManager.GetCollection<SyncAdapter>(this.ConnectionHandler, query);
        }

        public List<SyncAdapter> GetUserNewVersion(string tableName, long versionId, Guid userId)
        {
            var syncAdapterCommandBuilder = new SyncAdapterCommandBuilder();
            var query = syncAdapterCommandBuilder.GetUserNewVersion(tableName, versionId, userId);
            return DBManager.GetCollection<SyncAdapter>(this.ConnectionHandler, query);
        }

        public int DeprecateOlderVersion(Guid sourceId)
        {
            var syncAdapterCommandBuilder = new SyncAdapterCommandBuilder();
            var query = syncAdapterCommandBuilder.DeprecateOlderVersion(sourceId);
            return (int)DBManager.ExecuteNonQuery(this.ConnectionHandler, query);
        }
    }
    internal class SyncAdapterCommandBuilder
    {
        public string Exist(string tableName, string sourceId)
        {
            return string.Format("SELECT COUNT(*) FROM [CrossPlatform].[SyncAdapter] WHERE TableName = '{0}' AND SourceId = '{1}'", tableName, sourceId);
        }

        public string GetPublicNewVersion(string tableName, long versionId)
        {
            return string.Format("SELECT * FROM [CrossPlatform].[SyncAdapter] WHERE TableName = '{0}' AND Deprecated = 'false' AND VersionId > {1} AND UserId is null", tableName, versionId);
        }

        public string GetUserNewVersion(string tableName, long versionId, Guid userId)
        {
            return string.Format("SELECT * FROM [CrossPlatform].[SyncAdapter] WHERE TableName = '{0}' AND Deprecated = 'false' AND VersionId > {1} AND UserId = '{2}'", tableName, versionId, userId);
        }

        public string DeprecateOlderVersion(Guid sourceId)
        {
            return string.Format("UPDATE [CrossPlatform].[SyncAdapter] SET Deprecated = 'true' WHERE Type=" + (int)Enums.QueryTypes.Update + " AND SourceId = '{0}'", sourceId);
        }
    }
}
