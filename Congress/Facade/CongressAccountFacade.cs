using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressAccountFacade : CongressBaseFacade<CongressAccount>, ICongressAccountFacade
    {
        internal CongressAccountFacade()
        {
        }

        internal CongressAccountFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

   

       

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressAccountBo = new CongressAccountBO();
                var obj = congressAccountBo.Get(this.ConnectionHandler, keys);
                if (!congressAccountBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressAccount);
                if (
                    !PaymentComponenets.Instance.AccountTransactionalFacade(this.PaymentConnection)
                        .Delete(obj.AccountId))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressAccount);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId, Payment.DataStructure.Account account)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                account.IsExternal = true;
                if (!PaymentComponenets.Instance.AccountTransactionalFacade(this.PaymentConnection).Insert(account))
                    throw new Exception("خطایی در ذخیره حساب  وجود دارد");
                var congressAccount = new CongressAccount {AccountId = account.Id, CongressId = congressId};
                if (!new CongressAccountBO().Insert(this.ConnectionHandler, congressAccount))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressAccount);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
