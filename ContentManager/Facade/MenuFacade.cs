using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Common.DataStructure;
using Radyn.ContentManager.BO;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Facade.Interface;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
// ReSharper disable once RedundantUsingDirective
namespace Radyn.ContentManager.Facade
{
    internal sealed class MenuFacade : ContentManagerBaseFacade<Menu>, IMenuFacade
    {
        internal MenuFacade()
        {
        }

        internal MenuFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public bool Insert(Menu obj, HttpPostedFileBase fileBase)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (fileBase != null)
                    obj.ImageUrl =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Insert(fileBase);
                if (!new MenuBO().Insert(this.ConnectionHandler, obj))
                    throw new Exception("خطایی در ذخیره منو وجود دارد");


                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool Update(Menu obj, HttpPostedFileBase fileBase)
        {
            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                if (fileBase != null)
                {
                    var fileTransactionalFacade =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                    if (obj.ImageUrl.HasValue) fileTransactionalFacade.Update(fileBase, obj.ImageUrl.Value);
                    else obj.ImageUrl = fileTransactionalFacade.Insert(fileBase);
                }
                if (!new MenuBO().Update(this.ConnectionHandler, obj))
                    throw new Exception("خطایی در ویرایش منو میباشد");

                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();

                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<Menu> GetParents(Guid? congrssId)
        {
            try
            {

                return new MenuBO().GetParents(this.ConnectionHandler, congrssId);

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

      

        public IEnumerable<Menu> MenuTree(Guid? selected)
        {
            try
            {
                var menuBo = new MenuBO();
                var list = menuBo.Where(this.ConnectionHandler, x => x.ParentId == null);
                foreach (var variable in list)
                {
                    if (variable.Id == selected)
                        variable.Selected = true;
                    variable.Children.AddRange(menuBo.GetChildMenu(this.ConnectionHandler, variable.Id, selected));

                }
                return list;
            }
            catch (KnownException ex)
            {
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public IEnumerable<Menu> GetChildMenu(Guid parentId, Guid selected)
        {
            try
            {
                return new MenuBO().GetChildMenu(this.ConnectionHandler, parentId, selected);
            }
            catch (KnownException ex)
            {
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
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);

                var obj = new MenuBO().Get(this.ConnectionHandler, keys);
                if (obj == null) return false;
                if (obj.ImageUrl != null)
                    if (
                        !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Delete(obj.ImageUrl))
                        return false;
                var contents = new ContentBO().Where(ConnectionHandler, content => content.MenuId == obj.Id);
                if (contents.Count > 0)
                {
                    foreach (var content in contents)
                    {
                        content.MenuId = null;
                        if (!new ContentBO().Update(this.ConnectionHandler, content))
                            throw new Exception("خطایی در ویرایش منوی محتوا وجود دارد");
                    }

                }

                if (!new MenuBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در ویرایش منو وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

    }
}
