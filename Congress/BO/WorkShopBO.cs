using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class WorkShopBO : BusinessBase<WorkShop>
    {

        public override bool Update(IConnectionHandler connectionHandler, WorkShop obj)
        {
            if (!base.Update(connectionHandler, obj)) return false;
            var oldobj = this.Get(connectionHandler, obj.Id);
            if (oldobj.ProgramAttachId.HasValue && obj.ProgramAttachId == null)
                FileManagerComponent.Instance.FileFacade.Delete(oldobj.ProgramAttachId);
            if (oldobj.FileAttachId.HasValue && obj.FileAttachId == null)
                FileManagerComponent.Instance.FileFacade.Delete(oldobj.FileAttachId);
            return true;
        }

        public override WorkShop Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            var item = base.Get(connectionHandler, keys);
            if (item != null)
                SetCapacity(connectionHandler, new List<WorkShop>() { item }, item.CongressId);
            return item;
        }

        public override WorkShop GetLanuageContentSimple(IConnectionHandler connectionHandler, string culture, params object[] keys)
        {
            var item = base.GetLanuageContentSimple(connectionHandler, culture, keys);
            if (item != null)
                SetCapacity(connectionHandler, new List<WorkShop>() { item }, item.CongressId);
            return item;
        }

        public void SetCapacity(IConnectionHandler connectionHandler, List<WorkShop> list, Guid congressId)
        {
            var groupBy = new WorkShopUserBO().GroupBy(connectionHandler,
                new Expression<Func<WorkShopUser, object>>[] { x => x.WorkShopId },
                new GroupByModel<WorkShopUser>[]
                {
                    new GroupByModel<WorkShopUser>()
                    {
                        Expression = x => x.WorkShopId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                },
                x =>
                    x.WorkShop.CongressId == congressId && x.Status != (byte)Enums.WorkShopRezervState.Denial &&
                    x.Status != (byte)Enums.WorkShopRezervState.DenialPay);

            foreach (var item in list)
            {
                var firstOrDefault = groupBy.FirstOrDefault(x => x.WorkShopId == item.Id);
                int reservCount = firstOrDefault != null ? firstOrDefault.CountWorkShopId : 0;
                item.FreeCapicity = item.Capacity - reservCount;
                item.RezervCount = reservCount;


            }
        }

    }
}
