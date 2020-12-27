using System;
using System.Collections.Generic;
using System.Data;
using Radyn.ContentManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class ContainerFacade : WebDesignBaseFacade<Container>, IContainerFacade
    {
        internal ContainerFacade() { }

        internal ContainerFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressContainerBo = new ContainerBO();
                var obj = congressContainerBo.Get(this.ConnectionHandler, keys);
                if (!congressContainerBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف قالب وجود دارد");
                if (!ContentManagerComponent.Instance.ContainerTransactionalFacade(this.ContentManagerConnection).Delete(obj.ContainerId))
                    throw new Exception("خطایی در حذف قالب وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<ContentManager.DataStructure.Container> GetByWebSiteId(Guid websiteId)
        {
            try
            {

               
                return new ContainerBO().Select(this.ConnectionHandler,x=>x.WebSiteContainer,x=>x.WebId == websiteId);
                
            }
            catch (KnownException knownException)
            {

                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {

                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid websiteId, ContentManager.DataStructure.Container container)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                container.IsExternal = true;
                if (!ContentManagerComponent.Instance.ContainerTransactionalFacade(this.ContentManagerConnection).Insert(container))
                    throw new Exception("خطایی در ذخیره قالب وجود دارد");
                var congressContent = new Container { ContainerId = container.Id, WebId = websiteId };
                if (!new ContainerBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception("خطایی در ذخیره قالب وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
