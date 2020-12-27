using System;
using System.Collections.Generic;
using System.Web;
using Radyn.FAQ.DataStructure;
using Radyn.Framework;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IFAQFacade : IBaseFacade<DataStructure.FAQ>
{
    IEnumerable<FAQ.DataStructure.FAQ> GetByWebSiteId(Guid websiteId);
    IEnumerable<FAQ.DataStructure.FAQ> Search(Guid id, string value);
    bool Insert(Guid websiteId, FAQ.DataStructure.FAQ faq, FAQContent faqContent, HttpPostedFileBase image);
}
}
