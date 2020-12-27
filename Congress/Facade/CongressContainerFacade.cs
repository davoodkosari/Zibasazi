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
    internal sealed class CongressContainerFacade : CongressBaseFacade<CongressContainer>, ICongressContainerFacade
    {
        internal CongressContainerFacade()
        {
        }

        internal CongressContainerFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContainerBO = new CongressContainerBO();
                var obj = congressContainerBO.Get(this.ConnectionHandler, keys);
                if (!congressContainerBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressContainer);
                if (
                    !ContentManagerComponent.Instance.ContainerTransactionalFacade(this.ContentManagerConnection)
                        .Delete(obj.ContainerId))
                    throw new Exception("خطایی در حذف قالب وجود دارد");
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



        public bool Insert(Guid congressId, Container container)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                container.IsExternal = true;
                if (
                    !ContentManagerComponent.Instance.ContainerTransactionalFacade(this.ContentManagerConnection)
                        .Insert(container))
                    throw new Exception("خطایی در ذخیره قالب وجود دارد");
                var congressContent = new CongressContainer {ContainerId = container.Id, CongressId = congressId};
                if (!new CongressContainerBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressContainer);
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
