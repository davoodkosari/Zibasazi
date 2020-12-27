using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Payment.DataStructure;

namespace Radyn.Payment.BO
{
    internal class DiscountTypeBO : BusinessBase<DiscountType>
    {

        public override bool Insert(IConnectionHandler connectionHandler, DiscountType obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public override DiscountType Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var item = base.Get(connectionHandler, keys);
            item.IsAutoCode = new DiscountTypeAutoCodeBO().Any(connectionHandler, x => x.DiscountTypeId == item.Id);
            item.ForceCode = item.IsAutoCode || !string.IsNullOrEmpty(item.Code);
            return item;
        }

        public bool Update(IConnectionHandler connectionHandler,  DiscountType discountType, List<DiscountTypeAutoCode> discountTypeAutoCodes)
        {

            var discountTypeAutoCodeBo = new DiscountTypeAutoCodeBO();
            var typeAutoCodes = discountTypeAutoCodeBo.Where(connectionHandler,
                    x => x.DiscountTypeId == discountType.Id);
            if (discountTypeAutoCodes == null || !discountType.IsAutoCode)
            {
                foreach (var discountTypeAutoCode in typeAutoCodes)
                {
                     
                    if (!discountTypeAutoCodeBo.Delete(connectionHandler, discountTypeAutoCode.Id))
                        throw new Exception(Resources.Payment.ErrorInSaveDiscountCode);
                }
            }
            else
            {
                foreach (var autoCode in discountTypeAutoCodes)
                {
                    if (typeAutoCodes.All(x => x.Id != autoCode.Id))
                    {
                        if (!discountTypeAutoCodeBo.Insert(connectionHandler, autoCode))
                            throw new Exception(Resources.Payment.ErrorInSaveDiscountCode);
                    }
                }
                discountType.Code = string.Empty;
                discountType.Capacity = null;
                discountType.RemainCapacity = null;

            }
           
            if (!base.Update(connectionHandler, discountType))
                throw new Exception(Resources.Payment.ErrorInInsertDiscount);
            return true;

        }
        public bool Insert(IConnectionHandler connectionHandler,  DiscountType discountType, List<DiscountTypeAutoCode> discountTypeAutoCodes)
        {
            if (!this.Insert(connectionHandler, discountType))
                throw new Exception(Resources.Payment.ErrorInSaveDiscountCode);
            if (!discountType.IsAutoCode || discountTypeAutoCodes == null) return true;
            foreach (var autoCode in discountTypeAutoCodes)
            {
                autoCode.DiscountTypeId = discountType.Id;
                if (!new DiscountTypeAutoCodeBO().Insert(connectionHandler, autoCode))
                    throw new Exception(Resources.Payment.ErrorInSaveDiscountCode);
            }
         
            return true;

        }

       

    }
}
