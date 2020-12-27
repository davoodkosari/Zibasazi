using System;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.EnterpriseNode;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class VIPFacade : CongressBaseFacade<VIP>, IVIPFacade
    {
        internal VIPFacade()
        {
        }

        internal VIPFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public bool Insert(VIP vip,
            HttpPostedFileBase fileResume, HttpPostedFileBase file)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               
                var id = vip.Id;
                BOUtility.GetGuidForId(ref id);
                vip.Id = id;
                vip.EnterpriseNode.Id = vip.Id;
                if (fileResume != null)
                    vip.ResumeFileId =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Insert(fileResume);
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Insert(vip.EnterpriseNode, file))
                    return false;
                if (!new VIPBO().Insert(this.ConnectionHandler, vip))
                    throw new Exception(Resources.Congress.ErrorInSaveSpecialGuest);
                
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
               
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
               Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool Update(VIP vip, 
            HttpPostedFileBase fileResume, HttpPostedFileBase file)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
               if (fileResume != null)
                {
                    if (vip.ResumeFileId.HasValue)
                    {
                        if (
                            !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Update(fileResume,
                                    (Guid) vip.ResumeFileId))
                            throw new Exception(Resources.Congress.ErrorInSaveResume);
                    }
                    else
                        vip.ResumeFileId =
                            FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Insert(fileResume);
                }
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Update(vip.EnterpriseNode, file))
                    return false;
                
                if (!new VIPBO().Update(this.ConnectionHandler, vip))
                    throw new Exception(Resources.Congress.ErrorInEditSpecialGuest);
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
              
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
              
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
             
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public override bool Delete(params object[] keys)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new VIPBO().Get(this.ConnectionHandler, keys);
                if (!new VIPBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteSpecialGuest);
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Delete(keys))
                    return false;
                if (obj.ResumeFileId.HasValue)
                {
                    if (
                        !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Delete((Guid) obj.ResumeFileId))
                        throw new Exception(Resources.Congress.ErrorInDeleteResume);
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }
    }
}
