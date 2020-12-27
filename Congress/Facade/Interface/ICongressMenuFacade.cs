
using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressMenuFacade : IBaseFacade<CongressMenu>
    {
        bool Insert(Guid congressId, Menu menu,  HttpPostedFileBase file);
        List<Menu> MenuTree(Guid congressId, Guid selected);
        List<Menu> MenuTree(Guid congressId, string culture);



    }
}
