using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Gallery;

namespace Radyn.Congress.Facade
{
    internal sealed class CongessGalleryFacade : CongressBaseFacade<CongessGallery>, ICongessGalleryFacade
    {
        internal CongessGalleryFacade()
        {
        }

        internal CongessGalleryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }




       

        public bool Insert(Guid congressId,  Gallery.DataStructure.Gallery gallery,
            HttpPostedFileBase fileBase)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.GalleryConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                gallery.IsExternal = true;
                if (
                    !GalleryComponent.Instance.GalleryTransactinalFacade(this.GalleryConnection)
                        .Insert(gallery, fileBase))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var congessGallery = new CongessGallery {GalleryId = gallery.Id, CongressId = congressId};
                if (!new CongessGalleryBO().Insert(this.ConnectionHandler, congessGallery))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressGallery);
                this.ConnectionHandler.CommitTransaction();
                this.GalleryConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid congressId,  Gallery.DataStructure.Gallery gallery, HttpPostedFileBase fileBase,
            List<HttpPostedFileBase> httpPostedFileBases)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.GalleryConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                gallery.IsExternal = true;
                if (
                    !GalleryComponent.Instance.GalleryTransactinalFacade(this.GalleryConnection)
                        .Insert(gallery, fileBase, httpPostedFileBases))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var congessGallery = new CongessGallery { GalleryId = gallery.Id, CongressId = congressId };
                if (!new CongessGalleryBO().Insert(this.ConnectionHandler, congessGallery))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressGallery);
                this.ConnectionHandler.CommitTransaction();
                this.GalleryConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public List<KeyValuePair<string,string>> GetParents(Guid homaId)
        {
            try
            {
                
                return new CongessGalleryBO().SelectKeyValuePair(this.ConnectionHandler,x=>x.GalleryId,x=>x.Gallery.Title, x => x.CongressId == homaId&&x.Gallery.ParentGallery==null, new OrderByModel<CongessGallery>() { Expression = x => x.Gallery.Order });
              
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
                this.GalleryConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new CongessGalleryBO().Get(this.ConnectionHandler, keys);
                if (!new CongessGalleryBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressGallery);
                if (!GalleryComponent.Instance.GalleryTransactinalFacade(this.GalleryConnection).Delete(obj.GalleryId))
                    throw new Exception("خطایی در حذف گالری وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.GalleryConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
