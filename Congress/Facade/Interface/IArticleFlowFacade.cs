using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IArticleFlowFacade : IBaseFacade<ArticleFlow>
    {
        IEnumerable<ArticleFlow> GetArticleFlow(Guid congressId, Guid articelId, Guid? filterId=null,bool? isReferee=null, bool? isuser = null);
    }
}
