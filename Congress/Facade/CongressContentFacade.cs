using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressContentFacade : CongressBaseFacade<CongressContent>, ICongressContentFacade
    {
        internal CongressContentFacade()
        {
        }

        internal CongressContentFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

       
        

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContentBO = new CongressContentBO();
                var obj = congressContentBO.Get(this.ConnectionHandler, keys);
                if (!congressContentBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressContent);
                if (
                    !ContentManagerComponent.Instance.ContentTransactionalFacade(this.ContentManagerConnection)
                        .Delete(obj.ContentId))
                    throw new Exception("خطایی در حذف محتوا وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, Content content, ContentContent contentContent)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                content.IsExternal = true;
                if (
                    !ContentManagerComponent.Instance.ContentTransactionalFacade(this.ContentManagerConnection)
                        .Insert(content, contentContent))
                    throw new Exception("خطایی در ذخیره محتوا وجود دارد");
                var congressContent = new CongressContent {ContentId = content.Id, CongressId = congressId};
                if (!new CongressContentBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressContent);
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
