using System;
using System.Collections.Generic;
using Radyn.CrossPlatform.DataStructure;
using Radyn.Framework;

namespace Radyn.CrossPlatform.Facade.Interface
{
    public interface ISyncAdapterFacade : IBaseFacade<SyncAdapter>
    {

        bool Exist(string tableName, string sourceId);
        void InsertAsGroup(List<SyncAdapter> items);

        List<SyncAdapter> GetPublicNewVersion(string tableName, long versionId);
        List<SyncAdapter> GetUserNewVersion(string tableName, long versionId, Guid userId);
    }
}
