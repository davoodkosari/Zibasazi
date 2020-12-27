using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface ILanguageFacade : IBaseFacade<Language>
{
    IEnumerable<Common.DataStructure.Language> GetValidList(Guid websiteId);
    IEnumerable<Common.DataStructure.Language> GetByWebSiteId(Guid websiteId);
    bool Insert(Guid websiteId, Common.DataStructure.Language language, HttpPostedFileBase image);
}
}
