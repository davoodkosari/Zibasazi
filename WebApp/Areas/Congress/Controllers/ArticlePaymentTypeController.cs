using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.Web.Mvc.Utility;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class ArticlePaymentTypeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Where(x=>x.CongressId==this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult GetDetail(Guid Id)
        {
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Get(Id);
            if (articlePaymentType == null) return Content("false");
            return PartialView("PVDetails", articlePaymentType);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new ArticlePaymentType());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.GetLanuageContent(culture,Id);
            return PartialView("PVModify", languageContent);
        }
        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var paymentType = new ArticlePaymentType();
            try
            {
                this.RadynTryUpdateModel(paymentType);
                paymentType.CurrentUICultureName = collection["LanguageId"];
                paymentType.CongressId = this.Homa.Id;
                if (CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Insert(paymentType))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);

                    return this.SubmitRedirect(collection, new {Id = paymentType.Id});
                   
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticlePaymentType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(paymentType);
            }
        }

        

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(articlePaymentType);
                articlePaymentType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Update(articlePaymentType))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticlePaymentType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(articlePaymentType);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var paymentType = CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.ArticlePaymentTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/ArticlePaymentType/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/ArticlePaymentType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(paymentType);
            }
        }
    }
}