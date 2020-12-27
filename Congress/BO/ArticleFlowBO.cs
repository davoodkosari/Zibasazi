using System;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Utility;

namespace Radyn.Congress.BO
{
    internal class ArticleFlowBO : BusinessBase<ArticleFlow>
    {
       
        public bool AddFlow(IConnectionHandler connectionHandler, IConnectionHandler filemanagerConnectionHandler, Guid sender, Guid? reciverId, Guid articleId, byte? status = null, string comments = "", HttpPostedFileBase fileBase = null)
        {

            var flow = new ArticleFlow
            {
                ArticleId = articleId,
                SenderId = sender,
                ReceiverId = reciverId,
                SaveDate = DateTime.Now.ShamsiDate(),
                SaveTime = DateTime.Now.GetTime(),
                Remark = comments,
                Status = status
            };
            if (fileBase != null)
                flow.AttachmentFileId = FileManagerComponent.Instance.FileTransactionalFacade(filemanagerConnectionHandler).Insert(fileBase);
            var id = flow.Id;
            BOUtility.GetGuidForId(ref id);
            flow.Id = id;
            if (!this.Insert(connectionHandler, flow)) throw new Exception(Resources.Congress.ErrorInSaveArticleFlow);
            return true;

        }

       
    }
}
