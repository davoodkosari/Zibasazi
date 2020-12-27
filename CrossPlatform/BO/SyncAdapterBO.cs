using System;
using System.Collections.Generic;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.DA;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.BO
{
    internal class SyncAdapterBO : BusinessBase<SyncAdapter>
    {
        //public override bool Insert(IConnectionHandler connectionHandler, Pivot obj)
        //{
        //    var id = obj.Id;
        //    BOUtility.GetGuidForId(ref id);
        //    obj.Id = id;
        //    return base.Insert(connectionHandler, obj);
        //}

        public bool Exist(IConnectionHandler connectionHandler, string tableName, string sourceId)
        {
            var da = new SyncAdapterDA(connectionHandler);
            var count = da.Count(tableName, sourceId);
            if (count > 0)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<SyncAdapter> GetPublicNewVersion(IConnectionHandler connectionHandler, string tableName, long versionId)
        {
            var da = new SyncAdapterDA(connectionHandler);
            return da.GetPublicNewVersion(tableName, versionId);
        }

        public IEnumerable<SyncAdapter> GetUserNewVersion(IConnectionHandler connectionHandler, string tableName, long versionId, Guid userId)
        {
            var da = new SyncAdapterDA(connectionHandler);
            return da.GetUserNewVersion(tableName, versionId, userId);
        }

        public int DeprecateOldVersion(IConnectionHandler connectionHandler, Guid sourceId)
        {
            var da = new SyncAdapterDA(connectionHandler);
            return da.DeprecateOlderVersion(sourceId);
        }

    }
}
