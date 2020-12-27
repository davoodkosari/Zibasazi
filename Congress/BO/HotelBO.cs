using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class HotelBO : BusinessBase<Hotel>
    {
        public override bool Insert(IConnectionHandler connectionHandler, Hotel obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public override Hotel Get(IConnectionHandler connectionHandler, params object[] keys)
        {
           
            var item = base.Get(connectionHandler, keys);
            if (item != null)
                SetCapacity(connectionHandler, new List<Hotel>() { item }, item.CongressId);
            return item;
        }

        public override Hotel GetLanuageContent(IConnectionHandler connectionHandler, string culture, params object[] keys)
        {
            var item = base.GetLanuageContentSimple(connectionHandler, culture, keys);
            if (item != null)
                SetCapacity(connectionHandler, new List<Hotel>() { item }, item.CongressId);
            return item;
        }

        public void SetCapacity(IConnectionHandler connectionHandler, List<Hotel> list, Guid congressId)
        {
            var groupBy = new HotelUserBO().GroupBy(connectionHandler,
                new Expression<Func<HotelUser, object>>[] { x => x.HotelId },
                new GroupByModel<HotelUser>[]
                {
                    new GroupByModel<HotelUser>()
                    {
                        Expression = x => x.HotelId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                },
                x =>
                    x.Hotel.CongressId == congressId && x.Status != (byte)Enums.RezervState.Denial &&
                    x.Status != (byte)Enums.RezervState.DenialPay);
         
            foreach (var item in list)
            {
                var firstOrDefault = groupBy.FirstOrDefault(x => x.HotelId == item.Id);
                int reservCount = firstOrDefault!=null? firstOrDefault.CountHotelId:0;
                item.FreeCapicity = item.Capacity - reservCount;
                item.RezervCount = reservCount;


            }
        }

      
    }
}
