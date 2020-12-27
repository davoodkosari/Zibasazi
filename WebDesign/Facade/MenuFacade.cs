using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Common.DataStructure;
using Radyn.ContentManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.DataStructure;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class MenuFacade : WebDesignBaseFacade<Menu>, IMenuFacade
    {
        internal MenuFacade() { }

        internal MenuFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }



        public bool Insert(Guid websiteId, ContentManager.DataStructure.Menu menu,  HttpPostedFileBase file)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                menu.IsExternal = true;
                if (menu.Order == 0)
                {
                    var menus = new MenuBO().Max(this.ConnectionHandler,x=>x.WebSiteMenu.Order, x => x.WebId == websiteId);
                    menu.Order = menus == 0 ? 1 : menus + 1;
                }
                if (!ContentManagerComponent.Instance.MenuTransactionalFacade(this.ContentManagerConnection).Insert(menu,file))
                    throw new Exception("خطایی در ذخیره منو وجود دارد");
                var congressMenu = new Menu { MenuId = menu.Id, WebId = websiteId };
                if (!new MenuBO().Insert(this.ConnectionHandler, congressMenu))
                    throw new Exception("خطایی در ذخیره منو وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }




        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressMenuBo = new MenuBO();
                var obj = congressMenuBo.Get(this.ConnectionHandler, keys);
                if (!congressMenuBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف منو وجود دارد");
                if (!ContentManagerComponent.Instance.MenuTransactionalFacade(this.ContentManagerConnection).Delete(obj.MenuId))
                    throw new Exception("خطایی در حذف منو وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        public List<Radyn.ContentManager.DataStructure.Menu> MenuTree(Guid websiteId, string culture)
        {
            try
            {
                var menuBo = new MenuBO();
                return menuBo.GetParentsAdnChilds(this.ConnectionHandler, websiteId, culture);

            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<ContentManager.DataStructure.Menu> MenuTree(Guid websiteId)
        {
            try
            {
                var menuBo = new MenuBO();
                var list = menuBo.GetParents(this.ConnectionHandler, websiteId);
                var Id = menuBo.Select(ConnectionHandler, x => x.MenuId, x => x.WebId == websiteId);
                foreach (var variable in list)
                {
                    var child = ContentManagerComponent.Instance.MenuFacade.GetChildMenu(variable.Id, Guid.Empty);
                    foreach (var menu in child)
                    {
                        if (Id.Any(x => x.Equals(menu.Id)) && menu.Enabled)
                            variable.Children.Add(menu);
                    }

                }
                return list;
            }
            catch (KnownException knownException)
            {
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                throw new KnownException(ex.Message, ex);
            }

        }
    }
}
