using System;
using System.Collections.Generic;
using Radyn.Congress.DA;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class GroupRegisterDiscountBO : BusinessBase<GroupRegisterDiscount>
    {
        
        public override bool Insert(IConnectionHandler connectionHandler, GroupRegisterDiscount obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        protected override void CheckConstraint(IConnectionHandler connectionHandler, GroupRegisterDiscount item)
        {
            if (item.From > 0)
            {
                var groupRegisterDiscountDa = new GroupRegisterDiscountDA(connectionHandler);
                if(groupRegisterDiscountDa.AllowAdd(item)>0)
                    throw new Exception("در این محدوده تخفبف وجود دارد");
            }
            base.CheckConstraint(connectionHandler, item);
        }
        public IEnumerable<GroupRegisterDiscount> GetValidList(IConnectionHandler connectionHandler, Guid congressId)
        {

          
            return this.Where(connectionHandler, x =>
            
                x.CongressId == congressId&&
                x.Enable&& ((string.IsNullOrEmpty(x.EndDate.Trim())||(!string.IsNullOrEmpty(x.EndDate.Trim()) &&
                        x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0)) ||
                       ((string.IsNullOrEmpty(x.StartDate.Trim())||!string.IsNullOrEmpty(x.StartDate.Trim()) &&
                        x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0)))
            );
          

        }
        public GroupRegisterDiscount GetGroupDiscount(IConnectionHandler connectionHandler, Guid congressId, int count)
        {
          
            if (count == 0) return null;
            GroupRegisterDiscount registerDiscount = null;
            var groupRegisterDiscounts = this.GetValidList(connectionHandler,congressId);
            foreach (var groupRegisterDiscount in groupRegisterDiscounts)
            {
                if (!(groupRegisterDiscount.From <= count) || !(groupRegisterDiscount.To >= count)) continue;
                registerDiscount = groupRegisterDiscount;
                break;
            }
            return registerDiscount;
           
        }
    }
}
