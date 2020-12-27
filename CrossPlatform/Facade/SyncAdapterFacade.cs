using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.CrossPlatform.BO;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.Facade
{
    public sealed class SyncAdapterFacade : CrossPlatformBaseFacade<SyncAdapter>, ISyncAdapterFacade
    {
        public SyncAdapterFacade()
        {
        }

        public SyncAdapterFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public bool Exist(string tableName, string sourceId)
        {
            try
            {
                var exist = new SyncAdapterBO().Exist(this.ConnectionHandler, tableName, sourceId);
                return exist;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public void InsertAsGroup(List<SyncAdapter> items)
        {
            try
            {
                this.ConnectionHandler.StartTransaction();

                foreach (var item in items)
                {
                    if (new SyncAdapterBO().Insert(this.ConnectionHandler, item) == false)
                    {
                        throw new Exception("Saving Error");
                    }
                }

                this.ConnectionHandler.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<SyncAdapter> GetPublicNewVersion(string tableName, long versionId)
        {
            try
            {
                var list = new SyncAdapterBO().GetPublicNewVersion(this.ConnectionHandler, tableName, versionId);
                return list.ToList();
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public List<SyncAdapter> GetUserNewVersion(string tableName, long versionId, Guid userId)
        {
            try
            {
                var list = new SyncAdapterBO().GetUserNewVersion(this.ConnectionHandler, tableName, versionId, userId);
                return list.ToList();
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {

                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



    }
}
