using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment.BO;
using Radyn.Payment.DA;
using Radyn.Payment.DataStructure;
using Radyn.Payment.Facade.Interface;

namespace Radyn.Payment.Facade
{
    internal sealed class DiscountTypeFacade : PaymentBaseFacade<DiscountType>, IDiscountTypeFacade
    {
        internal DiscountTypeFacade() { }

        internal DiscountTypeFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        

        public bool Update(DiscountType discountType, List<DiscountTypeAutoCode> discountTypeAutoCodeFacades)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (!new DiscountTypeBO().Update(this.ConnectionHandler, discountType, discountTypeAutoCodeFacades))
                    throw new Exception("خطایی در ذخیره نوع تخفیف وجود دارد");
                this.ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();

                Log.Save(knownException.Message, LogType.ApplicationError);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(DiscountType discountType, string modelname, List<DiscountTypeSection> sectiontypes, List<DiscountTypeAutoCode> discountTypeAutoCodes)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);

                if (!new DiscountTypeBO().Update(this.ConnectionHandler, discountType, discountTypeAutoCodes))
                    throw new Exception("خطایی در ذخیره نوع تخفیف وجود دارد");
                var discountTypeSectionBo = new DiscountTypeSectionBO();
                var list = discountTypeSectionBo.GetByModelName(this.ConnectionHandler, modelname);
                foreach (var discountTypeSection in sectiontypes)
                {
                    if (list.Any(section => section.DiscountTypeId == discountTypeSection.DiscountTypeId && section.MoudelName == discountTypeSection.MoudelName && section.Section == discountTypeSection.Section)) continue;
                    if (!discountTypeSectionBo.Insert(this.ConnectionHandler, discountTypeSection))
                        throw new Exception(Resources.Payment.ErrorInInsertDiscount);

                }
                if (list != null)
                {
                    foreach (var typeSection in list.Where(x => x.DiscountTypeId == discountType.Id))
                    {
                        if (sectiontypes.Any(section => section.MoudelName == typeSection.MoudelName && section.Section == typeSection.Section))
                            continue;
                        if (!discountTypeSectionBo.Delete(this.ConnectionHandler, typeSection.DiscountTypeId, typeSection.MoudelName, typeSection.Section))
                            throw new Exception(Resources.Payment.ErrorInDeleteDiscount);
                    }
                }
                this.ConnectionHandler.CommitTransaction();

                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }



        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new DiscountTypeBO().Get(this.ConnectionHandler, keys);
                var discountTypeSections = new DiscountTypeSectionBO().Any(this.ConnectionHandler, x => x.DiscountTypeId == obj.Id);
                if (discountTypeSections)
                    throw new Exception("این نوع تخیف قابل حذف نیست زیرا در قسمت تخفیفات استفاده شده است");
                var sections = new TempDiscountBO().Any(this.ConnectionHandler, x => x.DiscountTypeId == obj.Id);
                if (sections)
                    throw new Exception("این نوع تخیف قابل حذف نیست زیرا در قسمت تخفیفات استفاده شده است");
                if (!new DiscountTypeBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف نوع تخفیف وجود دارد");
                var discountTypeAutoCodeBo = new DiscountTypeAutoCodeBO();
                var typeAutoCodes = discountTypeAutoCodeBo.Where(this.ConnectionHandler,
                           x => x.DiscountTypeId == obj.Id);
                foreach (var discountTypeAutoCode in typeAutoCodes)
                {
                    if (!discountTypeAutoCodeBo.Delete(this.ConnectionHandler, discountTypeAutoCode.Id))
                        throw new Exception("خطایی در ذخیره کد تخفیف وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public bool Insert(DiscountType discountType, List<DiscountTypeAutoCode> discountTypeAutoCodeFacades)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new DiscountTypeBO().Insert(this.ConnectionHandler, discountType, discountTypeAutoCodeFacades))
                    throw new Exception("خطایی در ذخیره نوع تخفیف وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public bool Insert(DiscountType discountType, List<DiscountTypeSection> sectiontypes, List<DiscountTypeAutoCode> discountTypeAutoCodes)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new DiscountTypeBO().Insert(this.ConnectionHandler, discountType, discountTypeAutoCodes))
                    throw new Exception("خطایی در ذخیره نوع تخفیف وجود دارد");
                foreach (var discountTypeSection in sectiontypes)
                {
                    discountTypeSection.DiscountTypeId = discountType.Id;
                    if (!new DiscountTypeSectionBO().Insert(this.ConnectionHandler, discountTypeSection))
                        throw new Exception("خطایی در ذخیره نوع تخفیف وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(knownException.Message, LogType.ApplicationError);
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
