using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade
{
    internal sealed class WorkShopUserFacade : CongressBaseFacade<WorkShopUser>, IWorkShopUserFacade
    {
        internal WorkShopUserFacade()
        {
        }

        internal WorkShopUserFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


        public bool UpdateList(Guid congressId, List<WorkShopUser> list)
        {
            var shopUserBo = new WorkShopUserBO();
            var entitiys = new ModelView.InFormEntitiyList<WorkShopUser>();
            bool result;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var transactionTransactionalFacade =
                    PaymentComponenets.Instance.TransactionTransactionalFacade(this.PaymentConnection);
                var userBo = new UserBO();
                foreach (var workShopUser1 in list)
                {
                    var workShopUser = shopUserBo.Get(this.ConnectionHandler, workShopUser1.UserId,
                        workShopUser1.WorkShopId);
                    if (workShopUser == null) continue;
                    workShopUser.Status = workShopUser1.Status;
                    if (workShopUser.Status == (byte)Enums.WorkShopRezervState.PayConfirm)
                    {
                        if (workShopUser.TransactionId != null)
                        {
                            if (!transactionTransactionalFacade.Done((Guid)workShopUser.TransactionId))
                                throw new Exception(Resources.Congress.ErrorInEditWorkShop);
                        }
                    }
                    if (!shopUserBo.Update(this.ConnectionHandler, workShopUser))
                        throw new Exception(Resources.Congress.ErrorInEditWorkShop);
                    var user = userBo.Get(this.ConnectionHandler, workShopUser1.UserId);
                    if (entitiys.All(x => x.obj.UserId != workShopUser.UserId))
                    {
                        entitiys.Add(
                        workShopUser,
                             Resources.Congress.WorkShopChangeStatusEmail,
                            Resources.Congress.WorkShopChangeStatusSMS
                        );
                    }


                    if (!user.ParentId.HasValue || entitiys.Any(x => x.obj.UserId == user.ParentId)) continue;
                    entitiys.Add(
                    new WorkShopUser()
                    {
                        UserId = (Guid)user.ParentId,
                        WorkShopId = workShopUser1.WorkShopId,
                        Status = workShopUser1.Status
                    },
                        Resources.Congress.WorkShopChangeStatusEmail,
                         Resources.Congress.WorkShopChangeStatusSMS
                    );
                }
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                result = true;
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
            try
            {
                if (result)
                    shopUserBo.InformWorkShopReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;
        }

        public KeyValuePair<bool, Guid> WorkShopUserInsert(Guid congressId, Guid workShopId, User parentUser,
            List<DiscountType> discountAttaches, string callBackurl, FormStructure formModel, List<Guid> users)
        {
            KeyValuePair<bool, Guid> keyValuePair;
            var entitiys = new ModelView.InFormEntitiyList<WorkShopUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var shopUserInsertGroup = new WorkShopUserBO().WorkShopUserInsert(this.ConnectionHandler,
                    this.PaymentConnection, this.FormGeneratorConnection, workShopId, parentUser, discountAttaches,
                    callBackurl, formModel, users);
                if (shopUserInsertGroup.Key && users.Count > 0)
                    entitiys.Add(

                       new WorkShopUser() { WorkShopId = workShopId, UserId = parentUser.Id },
                         Resources.Congress.WorkShopInsertEmail,
                         Resources.Congress.WorkShopInsertSMS
                    );
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                keyValuePair = new KeyValuePair<bool, Guid>(shopUserInsertGroup.Key, shopUserInsertGroup.Value);
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (keyValuePair.Key)
                    new WorkShopUserBO().InformWorkShopReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return keyValuePair;

        }




        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new WorkShopUserBO().Delete(this.ConnectionHandler, this.PaymentConnection, keys))
                    return false;
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



        public IEnumerable<WorkShopUser> Search(Guid id, byte? status, string registerDate, string searchvalue,
            FormStructure formStructure)
        {
            try
            {
                var workShopUserBO = new WorkShopUserBO();
                var outlist = workShopUserBO.Search(this.ConnectionHandler, id, status, registerDate, searchvalue, formStructure);
                return outlist;
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

        public Guid UpdateStatusAfterTransaction(Guid congressid, Guid userId, Guid tempId)
        {
            ModelView.ModifyResult<WorkShopUser> afterTransactionModel;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                afterTransactionModel = new WorkShopUserBO().UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection, userId, tempId);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();

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
            try
            {
                if (afterTransactionModel.SendInform)
                {
                    new WorkShopUserBO().InformWorkShopReserv(ConnectionHandler, congressid, afterTransactionModel.InformList);
                }
            }
            catch (Exception)
            {


            }
            return afterTransactionModel == null ? Guid.Empty : afterTransactionModel.TransactionId;

        }

        public bool WorkShopUserInsert(Guid congressId, WorkShopUser WorkShop, FormStructure formModel)
        {
            bool result = false;
            var entitiys = new ModelView.InFormEntitiyList<WorkShopUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                result = new WorkShopUserBO().WorkShopUserInsert(this.ConnectionHandler,
                   this.FormGeneratorConnection, WorkShop, formModel);
                entitiys.Add(

                    WorkShop,
                    Resources.Congress.WorkShopInsertEmail,
                    Resources.Congress.WorkShopInsertSMS
                );

                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
               
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
               this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
               this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                    new WorkShopUserBO().InformWorkShopReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;
        }

        public bool WorkShopUserUpdate(Guid congressId, WorkShopUser WorkShop, FormStructure formModel)
        {
            bool result = false;
            var entitiys = new ModelView.InFormEntitiyList<WorkShopUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                result = new WorkShopUserBO().WorkShopUserUpdate(this.ConnectionHandler,
                    this.FormGeneratorConnection, WorkShop, formModel);
                entitiys.Add(

                    WorkShop,
                    Resources.Congress.WorkShopInsertEmail,
                    Resources.Congress.WorkShopInsertSMS
                );

                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (result)
                    new WorkShopUserBO().InformWorkShopReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;
        }


        public IEnumerable<ModelView.ReportChartModel> ChartWorkShopCountByReserv(Guid congressId)
        {
            try
            {
                return new WorkShopUserBO().ChartWorkShopCountByReserv(this.ConnectionHandler, congressId);
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

        public IEnumerable<ModelView.ReportChartModel> ChartWorkShopCountStatus(Guid congressId)
        {
            try
            {
                return new WorkShopUserBO().ChartWorkShopCountStatus(this.ConnectionHandler, congressId);
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
