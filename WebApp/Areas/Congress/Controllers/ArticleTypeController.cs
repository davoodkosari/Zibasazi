using System;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ArticleTypeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Where(
                    x => x.CongressId == this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View(new ArticleType());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var articleType = new ArticleType();
            try
            {
                this.RadynTryUpdateModel(articleType);
                articleType.CongressId = this.Homa.Id;
                articleType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Insert(articleType))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = articleType.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticleType/Index");

            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return View(articleType);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var articleType = CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(articleType);
                articleType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Update(articleType))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticleType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(articleType);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var articleType = CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticleTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/ArticleType/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticleType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(articleType);
            }
        }
    }
}