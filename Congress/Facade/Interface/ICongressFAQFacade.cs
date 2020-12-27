using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.FAQ.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressFAQFacade : IBaseFacade<CongressFAQ>
{
    
    IEnumerable<FAQ.DataStructure.FAQ> Search(Guid congressId, string value);
    bool Insert(Guid congressId, FAQ.DataStructure.FAQ faq, FAQContent faqContent, HttpPostedFileBase image);
}
}
