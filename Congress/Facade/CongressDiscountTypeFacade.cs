using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment;
using Radyn.Payment.DataStructure;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressDiscountTypeFacade : CongressBaseFacade<CongressDiscountType>,
        ICongressDiscountTypeFacade
    {
        internal CongressDiscountTypeFacade()
        {
        }

        internal CongressDiscountTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public string FillTempAdditionalData(Guid congressId)
        {
            try
            {
               return new CongressDiscountTypeBO().FillTempAdditionalData(this.ConnectionHandler, congressId);
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

        public IEnumerable<DiscountType> GetByCongressId(Guid congressId)
        {
            try
            {
                var list = new List<DiscountType>();
                var congressAccounts = new CongressDiscountTypeBO().Where(this.ConnectionHandler,
                    x => x.CongressId == congressId);
                foreach (var congressAccount in congressAccounts)
                {
                    if (congressAccount.DiscountType == null) continue;
                    list.Add(congressAccount.DiscountType);
                }
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

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressDiscountTypeBo = new CongressDiscountTypeBO();
                var obj = congressDiscountTypeBo.Get(this.ConnectionHandler, keys);
                if (!congressDiscountTypeBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressDiscountype);
                if (
                    !PaymentComponenets.Instance.DiscountTypeTransactionalFacade(this.PaymentConnection)
                        .Delete(obj.DiscountTypeId))
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

        public bool Insert(Guid congressId, DiscountType discountType, List<DiscountTypeSection> sectiontypes,
            List<DiscountTypeAutoCode> discountTypeAutoCodes)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                discountType.IsExternal = true;
                if (!PaymentComponenets.Instance.DiscountTypeTransactionalFacade(this.PaymentConnection).Insert(discountType, sectiontypes, discountTypeAutoCodes))
                    throw new Exception("خطایی در ذخیره حساب  وجود دارد");

                var congressDiscountType = new CongressDiscountType
                {
                    DiscountTypeId = discountType.Id,
                    CongressId = congressId
                };
                if (!new CongressDiscountTypeBO().Insert(this.ConnectionHandler, congressDiscountType))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressDiscountype);
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

        public Guid UpdateStatusAfterTransactionGroupTemp(Guid userId, Guid id)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.PaymentConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ReservationConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var tempTransactionalFacade = PaymentComponenets.Instance.TempTransactionalFacade(this.PaymentConnection);
                var tr = tempTransactionalFacade.RemoveTempAndReturnTransaction(id);
                if (tr == null) return Guid.Empty;
                var byFilter = tempTransactionalFacade.Where(x => x.ParentId == id);
                if (byFilter.Any())
                {
                    var userBo = new UserBO();
                    var userBoothBo = new UserBoothBO();
                    var articleBo = new ArticleBO();
                    var hotelUserBo = new HotelUserBO();
                    var workShopUserBo = new WorkShopUserBO();
                    foreach (var temp in byFilter)
                    {
                        workShopUserBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                            userId, temp.Id);
                        hotelUserBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection, userId,
                            temp.Id);
                        articleBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                            this.FileManagerConnection, temp.Id,new ModelView.InFormEntitiyList<RefereeCartable>());
                        userBoothBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection, temp.Id);
                        userBo.UpdateStatusAfterTransaction(this.ConnectionHandler, this.PaymentConnection,
                            this.ReservationConnection, userId, temp.Id);
                        tempTransactionalFacade.RemoveTempAndReturnTransaction(temp.Id);
                    }
                }
                this.ConnectionHandler.CommitTransaction();
                this.PaymentConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                this.ReservationConnection.CommitTransaction();
                return tr.Id;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FileManagerConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.PaymentConnection.RollBack();
                this.FileManagerConnection.RollBack();
                this.ReservationConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
