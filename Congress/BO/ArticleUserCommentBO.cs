using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;
using System;
using System.Collections.Generic;

namespace Radyn.Congress.BO
{
    internal class ArticleUserCommentBO : BusinessBase<ArticleUserComment>
    {
        public override bool Insert(IConnectionHandler connectionHandler, ArticleUserComment obj)
        {
            Guid id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            obj.SaveDate = DateTime.Now.ShamsiDate();
            obj.SaveTime = DateTime.Now.GetTime();
            return base.Insert(connectionHandler, obj);
        }

        public bool UpdateConfirmAdmin(IConnectionHandler connectionHandler, Guid articleId, List<Guid> confirmList)
        {
            List<ArticleUserComment> comments = base.Where(connectionHandler, x => x.ArticleId == articleId && x.Id.In(confirmList));
            foreach (ArticleUserComment item in comments)
            {
                item.ConfirmAdmin = true;
                if (!base.Update(connectionHandler, item))
                {
                    throw new Exception(Resources.Congress.ErrorInUpdateComments);
                }
            }
            List<ArticleUserComment> unCheckedComments = base.Where(connectionHandler, x => x.ArticleId == articleId && x.Id.NotIn(confirmList));
            foreach (var item in unCheckedComments)
            {
                item.ConfirmAdmin = false;
                if (!base.Update(connectionHandler, item))
                {
                    throw new Exception(Resources.Congress.ErrorInUpdateComments);
                }
            }
            return true;
        }
    }
}
