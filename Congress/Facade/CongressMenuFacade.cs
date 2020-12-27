using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.ContentManager;
using Radyn.ContentManager.BO;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressMenuFacade : CongressBaseFacade<CongressMenu>, ICongressMenuFacade
    {
        internal CongressMenuFacade()
        {
        }

        internal CongressMenuFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public bool Insert(Guid congressId, Menu menu,  HttpPostedFileBase file)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                menu.IsExternal = true;
                if (menu.Order == 0)
                {
                    var congressMenus = new CongressMenuBO().Max(this.ConnectionHandler,z=>z.Menu.Order,
                        x => x.CongressId == congressId);
                    menu.Order = congressMenus + 1;
                }
                if (
                    !ContentManagerComponent.Instance.MenuTransactionalFacade(this.ContentManagerConnection)
                        .Insert(menu, file))
                    throw new Exception("خطایی در ذخیره منو وجود دارد");
                var congressMenu = new CongressMenu { MenuId = menu.Id, CongressId = congressId };
                if (!new CongressMenuBO().Insert(this.ConnectionHandler, congressMenu))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressMenu);
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public List<Menu> MenuTree(Guid congressId, Guid selected)
        {
            try
            {
                var menuBo = new CongressMenuBO();
                var list = menuBo.GetParents(this.ConnectionHandler, congressId);
                var Id = menuBo.Select(ConnectionHandler, x => x.MenuId, x => x.CongressId == congressId);
                foreach (var variable in list)
                {
                    var child = ContentManagerComponent.Instance.MenuFacade.GetChildMenu(variable.Id, selected);
                    foreach (var menu in child)
                    {
                        if (variable.Id == selected)
                        {
                            variable.Selected = true;
                        }
                        if (Id.Any(x => x.Equals(menu.Id)) && menu.Enabled)
                            variable.Children.Add(menu);
                    }

                }
                return list;
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

        public List<Menu> MenuTree(Guid congressId, string culture)
        {
            try
            {
                var menuBo = new CongressMenuBO();
                return menuBo.GetParentsAdnChilds(this.ConnectionHandler, congressId, culture);

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

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.ContentManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var congressMenuBO = new CongressMenuBO();
                var obj = congressMenuBO.Get(this.ConnectionHandler, keys);
                if (!congressMenuBO.Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressMenu);
                if (
                    !ContentManagerComponent.Instance.MenuTransactionalFacade(this.ContentManagerConnection)
                        .Delete(obj.MenuId))
                    throw new Exception("خطایی در حذف منو وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.ContentManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.ContentManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
