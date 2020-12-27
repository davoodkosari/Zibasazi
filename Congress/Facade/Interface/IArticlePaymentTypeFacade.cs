using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IArticlePaymentTypeFacade : IBaseFacade<ArticlePaymentType>
    {
        


        IEnumerable<ArticlePaymentType> GetValidList(Guid congressId);
    }
}
