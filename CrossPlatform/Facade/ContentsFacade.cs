using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.CrossPlatform.BO;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.Facade.Interface;
using Radyn.CrossPlatform.Tools;
using Radyn.FileManager;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.Facade
{
    internal sealed class ContentsFacade : CrossPlatformBaseFacade<Contents>, IContentsFacade
    {
        internal ContentsFacade()
        {
        }

     

        internal ContentsFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       

        public IEnumerable<Contents> GetListByCategory(Guid congressId, Guid categoryId)
        {
            var list = new ContentsBO().GetListByCategory(ConnectionHandler, congressId, categoryId);
            
            return list;
        }

       
        public bool Insert(Contents cnt, Guid? userId, HttpPostedFileBase @base)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (@base != null)
                {
                    cnt.Image = FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection).Insert(@base);
                }

                if (new ContentsBO().Insert(ConnectionHandler, cnt) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(cnt, Enums.QueryTypes.Insert, userId));

                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        public bool Update(Contents cnt, Guid? userId, HttpPostedFileBase @base)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (@base != null)
                {
                    if (cnt.Image.HasValue)
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Update(@base, cnt.Image.Value);
                    else
                        cnt.Image =
                            FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Insert(@base);
                }
                if (new ContentsBO().Update(ConnectionHandler, cnt) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().DeprecateOldVersion(ConnectionHandler, cnt.CongressId);
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(cnt, Enums.QueryTypes.Update, userId));


                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        public bool Delete(Guid Id, Guid? userId)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var contentObj = new ContentsBO().Get(ConnectionHandler, Id);
                if (new ContentsBO().Delete(ConnectionHandler, Id) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().DeprecateOldVersion(ConnectionHandler, contentObj.CongressId);
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(contentObj, Enums.QueryTypes.Delete, userId));

                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        private SyncAdapter createSyncAdapter(Contents obj, Enums.QueryTypes type, Guid? userId)
        {
            var syncObj = new SyncAdapter();

            var gen = new SyncCodeGenerator();
            string script = null;
            if (type == Enums.QueryTypes.Insert)
            {
                script = gen.CreateInsertQuery(obj, Enums.ClientsTableNames.Contents.ToString(),
                new List<SyncAction<Contents>>()
                {
                    new SyncAction<Contents>(x => x.Id),
                    new SyncAction<Contents>(x => x.CongressId),
                    new SyncAction<Contents>(x => x.Subject),
                    new SyncAction<Contents>(x => x.Body),
                    new SyncAction<Contents>(x => x.Summary),
                    new SyncAction<Contents>(x => x.ObserverCount),
                    new SyncAction<Contents>(x => x.RecordDate),
                    new SyncAction<Contents>(x => x.RecordTime),
                    new SyncAction<Contents>(x => x.CategoryId),
                    new SyncAction<Contents>(x => x.Image)
                });
            }
            else if (type == Enums.QueryTypes.Update)
            {
                script = gen.CreateUpdateQuery(obj, Enums.ClientsTableNames.Contents.ToString(),
                new List<SyncAction<Contents>>()
                {
                    new SyncAction<Contents>(x => x.Subject),
                    new SyncAction<Contents>(x => x.Body),
                    new SyncAction<Contents>(x => x.Summary),
                    new SyncAction<Contents>(x => x.CategoryId),
                    new SyncAction<Contents>(x => x.Image)
                },
                new List<SyncAction<Contents>>()
                {
                    new SyncAction<Contents>(x => x.Id)
                });
            }
            else if (type == Enums.QueryTypes.Delete)
            {
                script = gen.CreateDeleteQuery(obj, Enums.ClientsTableNames.Contents.ToString(),
                new List<SyncAction<Contents>>()
                {
                    new SyncAction<Contents>(x => x.Id)
                });
            }


            syncObj.Id = Guid.NewGuid();
            syncObj.SourceId = obj.Id.ToString();
            syncObj.CongressId = obj.CongressId;
            syncObj.Deprecated = false;
            syncObj.RecordDate = DateTime.Now;
            syncObj.Script = script;
            syncObj.TableName = Enums.ClientsTableNames.Contents.ToString();
            syncObj.UserId = userId;
            syncObj.Type = (int)type;

            return syncObj;
        }

    }
}
