using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.ContentManager.DA;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.ContentManager.BO
{
    public class MenuBO : BusinessBase<Menu>
    {
        public IEnumerable<Menu> GetParents(IConnectionHandler connectionHandler, Guid? congressId)
        {
            var da = new MenuDA(connectionHandler);
            return da.GetParents(congressId);
        }

        public IEnumerable<Menu> GetParentsAndChilds(IConnectionHandler connectionHandler, Guid? selected, Guid? congressId)
        {
            var da = new MenuDA(connectionHandler);
            var list = da.GetParentsAndChilds(congressId);
            if (selected == null) return list;
            foreach (var item in list.Where(item => item.Id == selected))
            {
                item.Selected = true;
            }
            return list;
        }

        public IEnumerable<Menu> GetChildMenu(IConnectionHandler connectionHandler, Guid parentId, Guid? selected)
        {

            var list = this.OrderBy(connectionHandler, x => x.Order, x => x.ParentId == parentId);
            var outlist = new List<Menu>();
            foreach (var menu in list)
            {
                if (menu.Id == selected)
                    menu.Selected = true;
                GetChild(connectionHandler, menu, selected);
                outlist.Add(menu);
            }
            return outlist;
        }
        public override bool Insert(IConnectionHandler connectionHandler, Menu obj)
        {

            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (obj.Order == 0)
                obj.Order = this.GetMaxOrder(connectionHandler) + 1;
            return base.Insert(connectionHandler, obj);
        }

        private int GetMaxOrder(IConnectionHandler connectionHandler)
        {
            var da = new MenuDA(connectionHandler);
            return da.GetMaxOrder();
        }
      
        private void GetChild(IConnectionHandler connectionHandler, Menu menuTree, Guid? selected)
        {

            var list = this.OrderBy(connectionHandler, x => x.Order, x => x.ParentId == menuTree.Id);
            foreach (var menu in list)
            {
                if (menu.Id == selected)
                    menu.Selected = true;
                menuTree.Children.Add(menu);
                GetChild(connectionHandler, menu, selected);
            }
        }

    }
}
