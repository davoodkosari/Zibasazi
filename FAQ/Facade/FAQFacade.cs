using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.FAQ.BO;
using Radyn.FAQ.DataStructure;
using Radyn.FAQ.Facade.Interface;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.FAQ.Facade
{
    internal sealed class FAQFacade : FAQBaseFacade<DataStructure.FAQ>, IFAQFacade
    {
        internal FAQFacade()
        {
        }

        internal FAQFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public bool Insert(DataStructure.FAQ obj, FAQContent content, HttpPostedFileBase image)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new FAQBO().Insert(this.ConnectionHandler, this.FileManagerConnection, obj, content, image))
                    throw new Exception("خطایی در ذخیره FAQ وجود دارد");
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

        public bool Update(DataStructure.FAQ obj, FAQContent faqContent, HttpPostedFileBase image)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!new FAQBO().Update(this.ConnectionHandler, this.FileManagerConnection, obj, faqContent, image))
                    throw new Exception("خطایی در ویرایش محتوای FAQ وجود دارد");
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


        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var faq = new FAQBO().Get(this.ConnectionHandler, keys);
                if (faq == null) return true;
                var list = new FAQContentBO().Where(this.ConnectionHandler, content => content.Id == faq.Id);
                foreach (var content in list)
                {
                    if (!new FAQContentBO().Delete(this.ConnectionHandler, content.Id, content.LanguageId))
                        throw new Exception("خطایی در حذف محتوا وجود دارد");
                }
                if (faq.ThumbnailId.HasValue)
                    if (
                        !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Delete(faq.ThumbnailId.Value))
                        throw new Exception("Could not delete Thumbnail.");
                if (!new FAQBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف محتوا وجود دارد");
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

        public IEnumerable<DataStructure.FAQ> Search(string value)
        {
            try
            {
                return new FAQBO().Search(this.ConnectionHandler, value);
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
    }
}
