using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IMenuFacade : IBaseFacade<Menu>
{
    bool Insert(Guid id, ContentManager.DataStructure.Menu menu, HttpPostedFileBase file);
    IEnumerable<ContentManager.DataStructure.Menu> MenuTree(Guid websiteId);
    List<Radyn.ContentManager.DataStructure.Menu> MenuTree(Guid congressId, string culture);
}
}
