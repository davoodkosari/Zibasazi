using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.ContentManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.BO
{
    internal class MenuBO : BusinessBase<Menu>
    {
        public List<ContentManager.DataStructure.Menu> GetParents(IConnectionHandler connectionHandler, Guid websiteId)
        {
            var enumerable =
                    this.Select(connectionHandler, x => x.MenuId, x => x.WebId == websiteId);
            var outlist = new List<ContentManager.DataStructure.Menu>();
            var menuFacade = ContentManagerComponent.Instance.MenuFacade;

            foreach (var guid in enumerable)
            {
                var menu = menuFacade.Get(guid);
                if(menu==null)continue;
                if (menu.ParentId != null || !menu.Enabled) continue;
                outlist.Add(menu);
            }
            return outlist.OrderBy(x => x.Order).ToList();
        }
        public List<Radyn.ContentManager.DataStructure.Menu> GetParentsAdnChilds(IConnectionHandler connectionHandler, Guid websiteId, string culture)
        {
            var enumerable = this.Select(connectionHandler, x => x.WebSiteMenu, x => x.WebId == websiteId && x.WebSiteMenu.Enabled, new OrderByModel<Menu>() { Expression = x => x.WebSiteMenu.Order });
            var outlist = new List<Radyn.ContentManager.DataStructure.Menu>();
            foreach (var menu in enumerable)
            {
               
                outlist.Add(menu);
            }
            return outlist;



        }
    }
}
