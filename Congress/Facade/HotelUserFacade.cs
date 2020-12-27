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
    internal sealed class HotelUserFacade : CongressBaseFacade<HotelUser>, IHotelUserFacade
    {
        internal HotelUserFacade()
        {
        }

        internal HotelUserFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new HotelUserBO().Delete(this.ConnectionHandler, this.PaymentConnection, keys))
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

        public bool UpdateList(Guid congressId, List<HotelUser> list)
        {
            bool result;
            var hotelUserBO = new HotelUserBO();
            var entitiys = new ModelView.InFormEntitiyList<HotelUser>();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var transactionTransactionalFacade =
                    PaymentComponenets.Instance.TransactionTransactionalFacade(this.PaymentConnection);
                var userBo = new UserBO();
                foreach (var hotelUser1 in list)
                {
                    var hotelUser = Get(hotelUser1.HotelId, hotelUser1.UserId);
                    if (hotelUser == null) continue;
                    hotelUser.Status = hotelUser1.Status;
                    if (hotelUser.Status == (byte) Enums.RezervState.PayConfirm)
                    {
                        if (hotelUser.TransactionId != null)
                            if (!transactionTransactionalFacade.Done((Guid) hotelUser.TransactionId)) return false;
                    }
                    if (!hotelUserBO.Update(this.ConnectionHandler, hotelUser))
                        throw new Exception(Resources.Congress.ErrorInEditHotelReserv);
                    var user = userBo.Get(this.ConnectionHandler, hotelUser1.UserId);
                    if (entitiys.All(x => x.obj.UserId != hotelUser.UserId))
                        entitiys.Add(hotelUser
, Resources.Congress.HotelChangeStatusEmail,Resources.Congress.HotelChangeStatusSMS);
                    if (!user.ParentId.HasValue || entitiys.Any(x => x.obj.UserId == user.ParentId)) continue;
                    entitiys.Add(
                    new HotelUser()
                    {
                        UserId = (Guid)user.ParentId,
                        HotelId = hotelUser1.HotelId,
                        Status = hotelUser1.Status
                    }
                        
                        ,
                       Resources.Congress.HotelChangeStatusEmail,
                       Resources.Congress.HotelChangeStatusSMS
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
                    hotelUserBO.InformHotelReserv(this.ConnectionHandler,congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;
        }



        public KeyValuePair<bool, Guid> HotelUserInsert(Guid congressId,Guid hotelId, User parentUser,
            List<DiscountType> discountAttaches, string callBackurl, FormGenerator.DataStructure.FormStructure formModel,
            int dayCount, List<Guid> userIdlist)
        {
            KeyValuePair<bool, Guid> keyValuePair;
            var entitiys = new ModelView.InFormEntitiyList<HotelUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var hotelUserInsertGroup = new HotelUserBO().HotelUserInsert(this.ConnectionHandler,
                    this.PaymentConnection,
                    this.FormGeneratorConnection, hotelId, parentUser, discountAttaches, callBackurl, formModel,
                    dayCount, userIdlist);
                if (hotelUserInsertGroup.Key && userIdlist.Count > 0)
                    entitiys.Add(
                    new HotelUser() {HotelId = hotelId, UserId = parentUser.Id},
                        Resources.Congress.HotelInsertEmail, Resources.Congress.HotelInsertSMS
                    );
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                keyValuePair = new KeyValuePair<bool, Guid>(hotelUserInsertGroup.Key, hotelUserInsertGroup.Value);
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
                    new HotelUserBO().InformHotelReserv(this.ConnectionHandler,congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return keyValuePair;


        }

        public bool HotelUserInsert(Guid congressId, HotelUser hotelUser, FormStructure formModel)
        {
            bool result = false;
            var entitiys = new ModelView.InFormEntitiyList<HotelUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                result = new HotelUserBO().HotelUserInsert(this.ConnectionHandler,
                    
                    this.FormGeneratorConnection, hotelUser, formModel);
                entitiys.Add(
                    hotelUser,
                    Resources.Congress.HotelInsertEmail, Resources.Congress.HotelInsertSMS
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
                    new HotelUserBO().InformHotelReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;

        }

        public bool HotelUserUpdate(Guid congressId, HotelUser hotelUser, FormStructure formModel)
        {
            bool result = false;
            var entitiys = new ModelView.InFormEntitiyList<HotelUser>();
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                result = new HotelUserBO().HotelUserUpdate(this.ConnectionHandler,

                    this.FormGeneratorConnection, hotelUser, formModel);
                entitiys.Add(
                    hotelUser,
                    Resources.Congress.HotelUpdateEmail, Resources.Congress.HotelUpdateSMS
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
                    new HotelUserBO().InformHotelReserv(this.ConnectionHandler, congressId, entitiys);
            }
            catch (Exception)
            {


            }
            return result;
        }


        public Guid UpdateStatusAfterTransaction(Guid congressid,Guid userId, Guid tempId)
        {
            ModelView.ModifyResult<HotelUser> afterTransactionModel;

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                afterTransactionModel = new HotelUserBO().UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection, userId,
                    tempId);
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
                    new HotelUserBO().InformHotelReserv(ConnectionHandler, congressid,afterTransactionModel.InformList);
            }
            catch (Exception)
            {

            }
            return afterTransactionModel.TransactionId;



        }

        public IEnumerable<HotelUser> Search(Guid hotelId, byte? status, string registerDate, string searchvalue,
            FormStructure formStructure)
        {
            try
            {
                var hotelUserBo = new HotelUserBO();
              
                var list = hotelUserBo.Search(this.ConnectionHandler, hotelId, status, registerDate, searchvalue,formStructure);
                return list;
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

        public IEnumerable<ModelView.ReportChartModel> CharHotelCountWithReserv(Guid congressId)
        {
            try
            {
                return new HotelUserBO().CharHotelCountWithReserv(this.ConnectionHandler, congressId);
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

        public IEnumerable<ModelView.ReportChartModel> CharHotelCountWithStatus(Guid congressId)
        {
            try
            {
                return new HotelUserBO().CharHotelCountWithStatus(this.ConnectionHandler, congressId);
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
