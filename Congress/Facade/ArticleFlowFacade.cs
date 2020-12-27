using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;
using Enums = Radyn.Congress.Tools.Enums;

namespace Radyn.Congress.Facade
{
    internal sealed class ArticleFlowFacade : CongressBaseFacade<ArticleFlow>, IArticleFlowFacade
    {
        internal ArticleFlowFacade()
        {
        }

        internal ArticleFlowFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }




        public IEnumerable<ArticleFlow> GetArticleFlow(Guid congressId, Guid articelId, Guid? filterId = null, bool? isReferee = null, bool? isuser = null)
        {
            try
            {
                var predicateBuilder = new PredicateBuilder<ArticleFlow>();
                predicateBuilder.And(x => x.ArticleId == articelId);

                if (filterId.HasValue)
                {
                  
                    predicateBuilder.And(x => x.ReceiverId == filterId || x.SenderId == filterId);
                    if (isuser.HasValue && isuser == true)
                    {
                        var guids = new RefereeBO().Select(this.ConnectionHandler, x => x.Id, x => x.CongressId == congressId);
                        if (guids.Any())
                            predicateBuilder.And(x => (x.SenderId == null || x.SenderId.NotIn(guids)) && (x.ReceiverId == null || x.ReceiverId.Value.NotIn(guids)));
                    }
                    if (isReferee.HasValue && isReferee.Value)
                    {
                        var users = new UserBO().Select(this.ConnectionHandler, c => c.Id, c => c.CongressId == congressId);
                        if (users.Any())
                            predicateBuilder.And(x => (x.SenderId == null || x.SenderId.NotIn(users)) && (x.ReceiverId == null || x.ReceiverId.Value.NotIn(users)));
                    }

                }

                var list = new ArticleFlowBO().OrderByDescending(this.ConnectionHandler, x => x.SaveDate + "" + x.SaveTime, predicateBuilder.GetExpression());
                var @select = new RefereeCartableBO().Select(base.ConnectionHandler,
                    new Expression<Func<RefereeCartable, object>>[] { x => x.Status, x => x.RefereeId },
                    i => i.ArticleId == articelId,
                    new OrderByModel<RefereeCartable>() { Expression = x => x.InsertDate, OrderType = OrderType.DESC });
                foreach (var item in list)
                {
                    if (item.Status != null)
                        item.LastRefreeView = ((Enums.FinalState)item.Status).GetDescriptionInLocalization();
                    else if (item.Sender != null)
                    {
                        var refereeCartable = @select.FirstOrDefault(i => i.RefereeId == item.SenderId);
                        if (refereeCartable != null)
                            item.LastRefreeView = ((Enums.FinalState)refereeCartable.Status).GetDescriptionInLocalization();
                    }
                    item.Remark = item.Remark.Replace("\r\n", "<br />");
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
    }
}
