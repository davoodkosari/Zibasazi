using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class CongressMenuBO : BusinessBase<CongressMenu>
    {
        public List<Menu> GetParents(IConnectionHandler connectionHandler, Guid congressId)
        {

            return
                this.Select(connectionHandler, x => x.Menu,
                    x => x.CongressId == congressId && x.Menu.Enabled && x.Menu.ParentId == null,
                    new OrderByModel<CongressMenu>() { Expression = x => x.Menu.Order });


        }

        public List<Menu> GetParentsAdnChilds(IConnectionHandler connectionHandler, Guid congressId, string culture)
        {
            var enumerable = this.Select(connectionHandler, x => x.Menu, x => x.CongressId == congressId && x.Menu.Enabled, new OrderByModel<CongressMenu>() { Expression = x => x.Menu.Order });
            var outlist = new List<Menu>();
            var facade = ContentManagerComponent.Instance.MenuFacade;
            foreach (var menu in enumerable)
            {
                if (!string.IsNullOrWhiteSpace(culture))
                    facade.GetLanuageContent(culture, menu);
                outlist.Add(menu);
            }
            return outlist;



        }






    }
}
