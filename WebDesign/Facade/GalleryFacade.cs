using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Web;
using Radyn.Common.Component;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Gallery;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class GalleryFacade : WebDesignBaseFacade<DataStructure.Gallery>, IGalleryFacade
    {
        internal GalleryFacade() { }

        internal GalleryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }


        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.GalleryConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var galleryBo = new GalleryBO();
                var obj = galleryBo.Get(this.ConnectionHandler, keys);
                if (!galleryBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف گالری وجود دارد");
                if (!GalleryComponent.Instance.GalleryTransactinalFacade(this.GalleryConnection).Delete(obj.GalleryId))
                    throw new Exception("خطایی در حذف گالری وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.GalleryConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        public IEnumerable<Gallery.DataStructure.Gallery> GetParents(Guid websiteId)
        {
            try
            {
               
                return new GalleryBO().Select(ConnectionHandler,x=>x.WebSiteGallery,x=>x.WebSiteGallery.ParentGallery==null&&x.WebId== websiteId);
               
               
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

        public bool Insert(Guid websiteId,  Gallery.DataStructure.Gallery gallery, HttpPostedFileBase image)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.GalleryConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                gallery.IsExternal = true;
                gallery.CreateDate = Utility.DateTimeUtil.ShamsiDate(DateTime.Now);
                if (!GalleryComponent.Instance.GalleryTransactinalFacade(this.GalleryConnection).Insert(gallery, image))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var congessGallery = new DataStructure.Gallery { GalleryId = gallery.Id, WebId = websiteId };
                if (!new GalleryBO().Insert(this.ConnectionHandler, congessGallery))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.GalleryConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.GalleryConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
