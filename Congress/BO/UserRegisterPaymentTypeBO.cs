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
    internal class UserRegisterPaymentTypeBO : BusinessBase<UserRegisterPaymentType>
    {
        public override bool Insert(IConnectionHandler connectionHandler, UserRegisterPaymentType obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            obj.BaseCapacity = obj.Capacity;
            return base.Insert(connectionHandler, obj);
        }

        public IEnumerable<ModelView.ReportChartModel> ChartRegisterTypeCount(IConnectionHandler connectionHandler, Guid congressId)
        {
            var listout = new List<ModelView.ReportChartModel>();
            var list = GroupBy(connectionHandler,
                new Expression<Func<UserRegisterPaymentType, object>>[] { x => x.Title },
                new GroupByModel<UserRegisterPaymentType>[]
                {
                    new GroupByModel<UserRegisterPaymentType>()
                    {
                        Expression = x => x.Id,
                        AggrigateFuntionType = AggrigateFuntionType.Count
                    },
                }, x => x.CongressId == congressId);
            var allType = this.Select(connectionHandler, new Expression<Func<UserRegisterPaymentType, object>>[]
            {
                x => x.Title,
                x => x.Id
            }, x => x.CongressId == congressId);
            foreach (var item in allType)
            {
                var first = list.FirstOrDefault(x => (x.Title is string) &&(string) x.Title == (string) item.Title);
                listout.Add(new ModelView.ReportChartModel()
                {
                    Count = first?.CountId ?? 0,
                    Value = (string)item.Title
                });
            }
            return listout;
        }

        public override bool Update(IConnectionHandler connectionHandler, UserRegisterPaymentType obj)
        {
            var org = this.Get(connectionHandler, obj.Id);
            if (org == null) return base.Update(connectionHandler, obj);

            if (obj.BaseCapacity == org.BaseCapacity) return base.Update(connectionHandler, obj);
            var diff = obj.BaseCapacity - org.BaseCapacity;
            var currCap = obj.Capacity + diff;
            if (currCap < 0)
                throw new KnownException("ظرفیت اولیه نمی تواند از ظرفیت مضرف شده کمتر باشد.");
            obj.Capacity = currCap;
            return base.Update(connectionHandler, obj);
        }

        public void SetDaysInfo(ref UserRegisterPaymentType obj, Dictionary<int, decimal> keyValuePairs)
        {
            var str = string.Empty;
            foreach (var keyValuePair in keyValuePairs)
            {
                if (!string.IsNullOrEmpty(str)) str += "-";
                str += keyValuePair.Key + "," + keyValuePair.Value;
            }

            obj.DaysInfo = str;
            if (string.IsNullOrEmpty(str)) return;
            obj.ValidAmount = string.Empty;
        }
    }
}
