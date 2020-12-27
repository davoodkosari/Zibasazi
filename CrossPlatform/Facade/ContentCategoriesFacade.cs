using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Permissions;
using System.Web;
using Radyn.CrossPlatform.BO;
using Radyn.CrossPlatform.DataStructure;
using Radyn.CrossPlatform.DA;
using Radyn.CrossPlatform.Facade.Interface;
using Radyn.CrossPlatform.Tools;
using Radyn.FileManager;
using Radyn.Framework.DbHelper;

namespace Radyn.CrossPlatform.Facade
{
    internal sealed class ContentCategoriesFacade : CrossPlatformBaseFacade<ContentCategories>, IContentCategoriesFacade
    {
        internal ContentCategoriesFacade()
        {
        }

      
        internal ContentCategoriesFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public bool Insert(ContentCategories cntCategory, HttpPostedFileBase @base)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (@base != null)
                {
                    cntCategory.Image = FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection).Insert(@base);
                }
                if (new ContentCategoriesBO().Insert(ConnectionHandler, cntCategory) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(cntCategory, Enums.QueryTypes.Insert, null));

                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        public bool Update(ContentCategories cntCategory, HttpPostedFileBase @base)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (@base != null)
                {
                    if (cntCategory.Image.HasValue)
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Update(@base, cntCategory.Image.Value);
                    else
                        cntCategory.Image =
                            FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Insert(@base);
                }

                if (new ContentCategoriesBO().Update(ConnectionHandler, cntCategory) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().DeprecateOldVersion(ConnectionHandler, cntCategory.CongressId);
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(cntCategory, Enums.QueryTypes.Update, null));


                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        public bool Delete(Guid Id)
        {
            try
            {
                ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var categoryObj = new ContentCategoriesBO().Get(ConnectionHandler, Id);
                if (new ContentCategoriesBO().Delete(ConnectionHandler, Id) == false)
                {
                    throw new Exception();
                }
                new SyncAdapterBO().DeprecateOldVersion(ConnectionHandler, categoryObj.CongressId);
                new SyncAdapterBO().Insert(ConnectionHandler, createSyncAdapter(categoryObj, Enums.QueryTypes.Delete, null));


                ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (Exception exp)
            {
                ConnectionHandler.RollBack();
            }

            return false;
        }

        private SyncAdapter createSyncAdapter(ContentCategories obj, Enums.QueryTypes type, Guid? userId)
        {
            var syncObj = new SyncAdapter();

            var gen = new SyncCodeGenerator();
            string script = null;
            if (type == Enums.QueryTypes.Insert)
            {
                script = gen.CreateInsertQuery(obj, Enums.ClientsTableNames.ContentCategories.ToString(),
                new List<SyncAction<ContentCategories>>()
                {
                    new SyncAction<ContentCategories>(x => x.Id),
                    new SyncAction<ContentCategories>(x => x.CongressId),
                    new SyncAction<ContentCategories>(x => x.Title),
                    new SyncAction<ContentCategories>(x => x.Description),
                    new SyncAction<ContentCategories>(x => x.OrderCategory),
                    new SyncAction<ContentCategories>(x => x.Image)
                });
            }
            else if (type == Enums.QueryTypes.Update)
            {
                script = gen.CreateUpdateQuery(obj, Enums.ClientsTableNames.ContentCategories.ToString(),
                new List<SyncAction<ContentCategories>>()
                {
                    new SyncAction<ContentCategories>(x => x.Description),
                    new SyncAction<ContentCategories>(x => x.Title),
                    new SyncAction<ContentCategories>(x => x.OrderCategory),
                    new SyncAction<ContentCategories>(x => x.Image)
                },
                new List<SyncAction<ContentCategories>>()
                {
                    new SyncAction<ContentCategories>(x => x.Id)
                });
            }
            else if (type == Enums.QueryTypes.Delete)
            {
                script = gen.CreateDeleteQuery(obj, Enums.ClientsTableNames.ContentCategories.ToString(),
                new List<SyncAction<ContentCategories>>()
                {
                    new SyncAction<ContentCategories>(x => x.Id)
                });
            }

            syncObj.Id = Guid.NewGuid();
            syncObj.SourceId = obj.Id.ToString();
            syncObj.CongressId = obj.CongressId;
            syncObj.Deprecated = false;
            syncObj.RecordDate = DateTime.Now;
            //syncObj.Script = new SyncCodeGenerator().GetContentCategoryQuery(type, obj);
            syncObj.Script = script;
            syncObj.TableName = Enums.ClientsTableNames.ContentCategories.ToString();
            syncObj.UserId = userId;
            syncObj.Type = (int)type;

            return syncObj;
        }
    }
}
