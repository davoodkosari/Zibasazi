using System;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Congress;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressLanguageController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.Select(x=>x.Language,x=>x.CongressId==this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(string Id)
        {
            ViewBag.Id = Id;
            return View();

        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Create(FormCollection collection)
        {
            var language = new Language();

            try
            {
                this.RadynTryUpdateModel(language, collection);

                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.Insert(this.Homa.Id, language, image))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressLanguage/Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressLanguage/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(language);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Edit(string Id, FormCollection collection)
        {
            var language = CommonComponent.Instance.LanguageFacade.Get(Id);

            try
            {

                this.RadynTryUpdateModel(language, collection);
                HttpPostedFileBase image = null;
                if (Session["Image"] != null)
                {
                    image = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }

                if (CommonComponent.Instance.LanguageFacade.Update(language, image))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressLanguage/Index");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressLanguage/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(language);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        [RadynAuthorize]
        public ActionResult Delete(string Id, FormCollection collection)
        {

          
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.Delete(this.Homa.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressLanguage/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressLanguage/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }



        public ActionResult CultureBar()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressLanguageFacade.GetValidList(this.Homa.Id);
            return PartialView("LanguageBar", list);
        }


    }
}
