using Radyn.Congress.DataStructure;
using Radyn.Framework;
using System;
using System.Collections.Generic;

namespace Radyn.Congress.Facade.Interface
{
    public interface IArticleUserCommentFacade : IBaseFacade<ArticleUserComment>
    {
        bool UpdateConfirmAdmin(Guid articleId, List<Guid> confirmList);
    }
}
