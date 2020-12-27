using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class BoothBO : BusinessBase<Booth>
    {
        public override bool Insert(IConnectionHandler connectionHandler, Booth obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);

        }
        public List<Booth> GetunusedByUserId(IConnectionHandler connectionHandler, Guid userId, Guid congressId)
        {
            var predicateBuilder=new PredicateBuilder<Booth>();
            var @select=new UserBoothBO().Select(connectionHandler,x=>x.BoothId,x=>x.UserId==userId&&x.Booth.CongressId==congressId);
            if(select.Any())
                predicateBuilder.And(x=>x.Id.NotIn(select));
            predicateBuilder.And(x=>x.CongressId==congressId);
            return OrderBy(connectionHandler,x=>x.Code,  predicateBuilder.GetExpression());
           
        }

        public override Booth Get(IConnectionHandler connectionHandler, params object[] keys)
        {
            
            var item = base.Get(connectionHandler, keys);
            if (item != null)
                SetCapacity(connectionHandler, new List<Booth>() { item }, item.CongressId);
            return item;
        }

        public override Booth GetLanuageContent(IConnectionHandler connectionHandler, string culture, params object[] keys)
        {
            var getLanuageContent=base.GetLanuageContent(connectionHandler, culture, keys);
            if(getLanuageContent!=null)
                SetCapacity(connectionHandler,new List<Booth>() {getLanuageContent},getLanuageContent.CongressId );
            return getLanuageContent;
        }

        public void SetCapacity(IConnectionHandler connectionHandler,List<Booth> list, Guid congressId)
        {
            var groupBy = new UserBoothBO().GroupBy(connectionHandler,
                new Expression<Func<UserBooth, object>>[] { x => x.BoothId },
                new GroupByModel<UserBooth>[]
                {
                    new GroupByModel<UserBooth>()
                    {
                        Expression = x => x.BoothId,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                },
                x =>
                    x.Booth.CongressId == congressId && x.Status != (byte)Enums.RezervState.Denial &&
                    x.Status != (byte)Enums.RezervState.DenialPay);
            
            foreach (var item in list)
            {
                var firstOrDefault = groupBy.FirstOrDefault(x => x.BoothId == item.Id);
                int reservCount = firstOrDefault!=null? firstOrDefault.CountBoothId:0;
                item.FreeCapicity = item.ReservCapacity - reservCount;
                item.RezervCount = reservCount;


            }
        }
      
    }
}
