using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.FormGenerator;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;

namespace Radyn.Congress.Facade
{
    internal sealed class UserBoothFacade : CongressBaseFacade<UserBooth>, IUserBoothFacade
    {
        internal UserBoothFacade()
        {
        }

        internal UserBoothFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

   
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new UserBoothBO().Delete(this.ConnectionHandler, this.PaymentConnection, keys))
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

        public bool UserBoothInsert(Guid congressId,UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl,
            FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            ModelView.ModifyResult<UserBooth> modifyResult;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                modifyResult=new UserBoothBO().UserBoothInsert(this.ConnectionHandler, this.PaymentConnection,
                    this.FormGeneratorConnection, this.EnterpriseNodeConnection, userBooth, discountAttaches,callBackurl, formModel, boothOfficers);
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
               

            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (modifyResult.SendInform)
                {
                    new UserBoothBO().InformUserboothReserv(ConnectionHandler, congressId, modifyResult.InformList);
                }
            }
            catch (Exception)
            {


            }

            return modifyResult.Result;
        }
        public bool UserBoothInsert(Guid congressId, UserBooth userBooth, 
          FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            ModelView.ModifyResult<UserBooth> modifyResult;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                modifyResult = new UserBoothBO().UserBoothInsert(this.ConnectionHandler, 
                    this.FormGeneratorConnection, this.EnterpriseNodeConnection, userBooth, formModel, boothOfficers);
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
               this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (modifyResult.SendInform)
                {
                    new UserBoothBO().InformUserboothReserv(ConnectionHandler, congressId, modifyResult.InformList);
                }
            }
            catch (Exception)
            {


            }

            return modifyResult.Result;
        }
        internal IEnumerable<ModelView.ReportChartModel> ChartNumberBoothsByDivision(Guid congressId)
        {
            try
            {
                return new UserBoothBO().ChartNumberBoothsByDivision(this.ConnectionHandler, congressId);
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

        public bool UserBoothUpdate(Guid congressId,UserBooth userBooth, List<DiscountType> discountAttaches, string callBackurl,
            FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            ModelView.ModifyResult<UserBooth> modifyResult;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                modifyResult = new UserBoothBO().UserBoothUpdate(this.ConnectionHandler, this.PaymentConnection,
                    this.FormGeneratorConnection, this.EnterpriseNodeConnection, userBooth, discountAttaches,
                    callBackurl, formModel, boothOfficers);
                    
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                
              
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.PaymentConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (modifyResult.SendInform)
                {
                    new UserBoothBO().InformUserboothReserv(ConnectionHandler, congressId, modifyResult.InformList);
                }
            }
            catch (Exception)
            {


            }
            return modifyResult.Result;
        }
        public bool UserBoothUpdate(Guid congressId, UserBooth userBooth, 
           FormStructure formModel, List<BoothOfficer> boothOfficers)
        {
            ModelView.ModifyResult<UserBooth> modifyResult;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                modifyResult = new UserBoothBO().UserBoothUpdate(this.ConnectionHandler, 
                    this.FormGeneratorConnection, this.EnterpriseNodeConnection, userBooth, formModel, boothOfficers);

                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
               this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (modifyResult.SendInform)
                {
                    new UserBoothBO().InformUserboothReserv(ConnectionHandler, congressId, modifyResult.InformList);
                }
            }
            catch (Exception)
            {


            }
            return modifyResult.Result;
        }
        public IEnumerable<UserBooth> Search(Guid id, byte? status, string registerDate, string searchvalue,
            FormStructure formStructure)
        {
            try
            {
                var userBoothBo = new UserBoothBO();
                var outlist = userBoothBo.Search(this.ConnectionHandler, searchvalue, id, status, registerDate, formStructure);
                
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

        public Guid UpdateStatusAfterTransaction(Guid congressId,Guid user, Guid boothId)
        {
            var userBoothBo = new UserBoothBO();
            ModelView.ModifyResult<UserBooth> afterTransactionModel;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var userBooth = userBoothBo.Get(this.ConnectionHandler, user, boothId);
                if (userBooth.TempId == null) return Guid.Empty;
                afterTransactionModel = userBoothBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                    (Guid) userBooth.TempId);
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
                    userBoothBo.InformUserboothReserv(ConnectionHandler, congressId, afterTransactionModel.InformList);
                }

            }
            catch (Exception)
            {

            }
            return afterTransactionModel == null ? Guid.Empty : afterTransactionModel.TransactionId;
            
        }

       

        

        public bool UpdateList(Guid congressId,List<UserBooth> list)
        {
            var userBoothBo = new UserBoothBO();
            var keyValuePairs = new ModelView.InFormEntitiyList<UserBooth>();
            bool result;
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                foreach (var rezerUserModel in list)
                {
                    var transactionTransactionalFacade =
                        PaymentComponenets.Instance.TransactionTransactionalFacade(this.PaymentConnection);
                    var obj = userBoothBo.Get(this.ConnectionHandler, rezerUserModel.UserId, rezerUserModel.BoothId);
                    if (obj == null) continue;
                    obj.Status = rezerUserModel.Status;
                    if (obj.Status == (byte) Enums.RezervState.PayConfirm)
                    {
                        if (obj.TransactionId != null)
                        {
                            if (!transactionTransactionalFacade.Done((Guid) obj.TransactionId))
                                throw new Exception("خطایی در ثبت تراکنش وجود دارد");
                        }
                    }

                    if (!userBoothBo.Update(this.ConnectionHandler, obj))
                        throw new Exception("خطایی در ویرایش رزرو غرفه کاربر وجود دارد");
                    keyValuePairs.Add( obj,Resources.Congress.BoothChangeStatusEmail ,Resources.Congress.BoothChangeStatusSMS);
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
                    userBoothBo.InformUserboothReserv(this.ConnectionHandler, congressId, keyValuePairs);
            }
            catch (Exception)
            {


            }
            return result;
        }

        public KeyValuePair<bool, Guid> InsertGuest(EnterpriseNode.DataStructure.EnterpriseNode enterpriseNode,
            List<Guid> boothIdlist, HttpPostedFileBase file, List<DiscountType> discountAttaches,
            string callBackurl, FormGenerator.DataStructure.FormStructure postFormData, Guid congressId)
        {

            var keyValuePairs = new ModelView.InFormEntitiyList<UserBooth>();
            var guid = Guid.Empty;
            var userBoothBo = new UserBoothBO();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var additionalData = new CongressDiscountTypeBO().FillTempAdditionalData(this.ConnectionHandler, congressId);
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Insert(enterpriseNode, file)) return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                postFormData.RefId = enterpriseNode.Id.ToString();
                if (
                    !FormGeneratorComponent.Instance.FormDataTransactionalFacade(this.FormGeneratorConnection)
                        .ModifyFormData(postFormData))
                    throw new Exception(Resources.Congress.ErrorInSaveBoothReserv);
                var boothBo = new BoothBO();
                var amount = boothBo.Sum(ConnectionHandler, x => x.ValidCost, x => x.Id.In(boothIdlist));
                
                if (amount.ToDecimal() > 0)
                {
                    var firstOrDefault = boothIdlist.FirstOrDefault();
                    var booth = boothBo.Get(this.ConnectionHandler, firstOrDefault);
                    var temp = new Temp
                    {
                        PayerId = enterpriseNode.Id,
                        CallBackUrl = callBackurl + enterpriseNode.Id,
                        PayerTitle = enterpriseNode.DescriptionField,
                        Description = Resources.Congress.PaymentBoothReserv,
                        Amount =
                            new CongressDiscountTypeBO().CalulateAmountNew(this.PaymentConnection, amount.ToDecimal(),
                                discountAttaches),
                        CurrencyType = (byte) booth.CurrencyType.ToEnum<Radyn.Common.Definition.Enums.CurrencyType>(),
                        AdditionalData = additionalData
                    };
                   
                    if (
                        !PaymentComponenets.Instance.TempTransactionalFacade(this.PaymentConnection)
                            .Insert(temp, discountAttaches)) return new KeyValuePair<bool, Guid>(false, Guid.Empty);
                    guid = temp.Id;
                }
                foreach (var guid1 in boothIdlist)
                {
                    var userBooth = new UserBooth()
                    {
                        UserId = enterpriseNode.Id,
                        BoothId = guid1,
                        TempId = guid != Guid.Empty ? guid : (Guid?) null,
                    };
                    if (!userBoothBo.Insert(this.ConnectionHandler, userBooth))
                        throw new Exception(Resources.Congress.ErrorInSaveBoothReserv);
                    keyValuePairs.Add(userBooth,
                         Resources.Congress.BoothInsertEmail , Resources.Congress.BoothInsertSMS);

                }

                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();


            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FormGeneratorConnection.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                userBoothBo.InformUserboothReserv(this.ConnectionHandler, congressId,keyValuePairs);
            }
            catch (Exception)
            {


            }
            return new KeyValuePair<bool, Guid>(true, guid);
        }

        public void UpdateStatusAfterTransactionGuest(Guid congressId,Guid id)
        {
            var userBoothBo = new UserBoothBO();
            var list = new ModelView.InFormEntitiyList<UserBooth>();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var guestbooths = userBoothBo.Where(this.ConnectionHandler, guestbooth => guestbooth.UserId == id);
                var tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(this.PaymentConnection);
                var firstOrDefault = guestbooths.FirstOrDefault(x => x.TempId != null);
                if (firstOrDefault == null || firstOrDefault.TempId == null) return;
                var tr = tempTransactionalFacade.RemoveTempAndReturnTransaction((Guid) firstOrDefault.TempId);
                if (tr == null) return;
                foreach (var guestbooth in guestbooths)
                {
                    guestbooth.TransactionId = tr.Id;
                    guestbooth.TempId = null;
                    if (tr.PreDone)
                    {
                        guestbooth.Status = (byte) Enums.RezervState.Pay;
                        list.Add(guestbooth,Resources.Congress.BoothPaymentEmail , Resources.Congress.BoothPaymentSMS);
                    }
                    if (!userBoothBo.Update(this.ConnectionHandler, guestbooth))
                        throw new Exception(Resources.Congress.ErrorInEditBoothReserv);
                }
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
                userBoothBo.InformUserboothReserv(this.ConnectionHandler, congressId, list);
            }
            catch (Exception)
            {


            }
        }


        public IEnumerable<ModelView.ReportChartModel> ChartNumberStandsWithReservationSeparation(Guid congressId)
        {
            try
            {
                return new UserBoothBO().ChartNumberStandsWithReservationSeparation(this.ConnectionHandler, congressId);
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
