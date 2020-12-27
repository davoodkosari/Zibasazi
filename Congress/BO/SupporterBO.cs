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
    internal class SupporterBO : BusinessBase<Supporter>
    {
        public override bool Insert(IConnectionHandler connectionHandler, Supporter obj)
        {


            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (obj.Sort == 0)
                obj.Sort = (short)(this.Max(connectionHandler, x => x.Sort) + 1);

            if (!obj.WebSite.Contains("http") && !obj.WebSite.Contains("https"))
            {
                obj.WebSite = "http://" + obj.WebSite;
            }
            return base.Insert(connectionHandler, obj);
        }




        public override bool Update(IConnectionHandler connectionHandler, Supporter obj)
        {
            if (!obj.WebSite.Contains("http") && !obj.WebSite.Contains("https"))
            {
                obj.WebSite = "http://" + obj.WebSite;
            }
            return base.Update(connectionHandler, obj);
        }

        public IEnumerable<ModelView.ReportChartModel> ChartNumberStandsWithReservationSeparation(IConnectionHandler connectionHandler, Guid congressId)
        {
            var models = new List<Tools.ModelView.ReportChartModel>();
            var list = GroupBy(connectionHandler,
                new Expression<Func<Supporter, object>>[] { c => c.SupportTypeId },
                new GroupByModel<Supporter>[]
                {
                    new GroupByModel<Supporter>()
                    {
                        Expression = c=>c.SupportTypeId,
                        AggrigateFuntionType = AggrigateFuntionType.Count

                    },
                }, c => c.CongressId == congressId);

            var refList = new SupportTypeBO().Select(connectionHandler,
                new Expression<Func<SupportType, object>>[]
                {
                    x => x.Id,
                    x =>x.Title
                     }, x => x.CongressId == congressId);

            foreach (var o in refList)
            {
                if (!(o.Title is string)) continue;
                var model = new Tools.ModelView.ReportChartModel { Value = o.Title };
                var firstOrDefault = list.FirstOrDefault(x => x.SupportTypeId is short && x.SupportTypeId == o.Id);
                if (firstOrDefault != null && firstOrDefault.CountSupportTypeId is int)
                    model.Count = firstOrDefault.CountSupportTypeId;
                else
                    model.Count = 0;
                models.Add(model);
            }

            return models;
        }
    }
}
