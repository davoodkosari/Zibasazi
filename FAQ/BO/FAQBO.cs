using System;
using System.Collections.Generic;
using System.Web;
using Radyn.FAQ.DA;
using Radyn.FAQ.DataStructure;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.FAQ.BO
{
    internal class FAQBO : BusinessBase<FAQ.DataStructure.FAQ>
    {
        public override bool Insert(IConnectionHandler connectionHandler, DataStructure.FAQ obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public IEnumerable<DataStructure.FAQ> Search(IConnectionHandler connectionHandler, string value)
        {
            var faqda = new FAQDA(connectionHandler);
            return faqda.Search(value);
        }
        public bool Insert(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, DataStructure.FAQ obj, FAQContent content, HttpPostedFileBase image)
        {
            if (image != null)
                obj.ThumbnailId = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerconnection).Insert(image);
            if (!this.Insert(connectionHandler, obj))
                throw new Exception("خطایی در ذخیره FAQ وجود دارد");
            if (content == null) return true;
            content.Id = obj.Id;
            if (!new FAQContentBO().Insert(connectionHandler, content))
                throw new Exception("خطایی در ذخیره محتوای FAQ وجود دارد");
            return true;
        }
        public bool Update(IConnectionHandler connectionHandler, IConnectionHandler filemanagerconnection, DataStructure.FAQ obj, FAQContent faqContent, HttpPostedFileBase image)
        {
            if (image != null)
            {
                var fileTransactionalFacade = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerconnection);
                if (obj.ThumbnailId.HasValue)
                    fileTransactionalFacade.Update(image, obj.ThumbnailId.Value);
                else obj.ThumbnailId = fileTransactionalFacade.Insert(image);
            }
            if (!this.Update(connectionHandler, obj))
                throw new Exception("خطایی در ویرایش محتوای FAQ وجود دارد");
            if (faqContent == null) return true;
            var faqContentBo = new FAQContentBO();
            if (faqContent.Id == Guid.Empty)
            {
                faqContent.Id = obj.Id;
                if (!faqContentBo.Insert(connectionHandler, faqContent))
                    throw new Exception("خطایی در ذخیره محتوای FAQ وجود دارد");

            }
            else if (!faqContentBo.Update(connectionHandler, faqContent))
                throw new Exception("خطایی در ویرایش محتوای FAQ وجود دارد");
            return true;

        }
    }
}
