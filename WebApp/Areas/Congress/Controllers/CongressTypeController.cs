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
    public class CongressTypeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.OrderBy(x=>x.Title);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(int Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View(new CongressType(){Enable = true});
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var CongressType = new CongressType();
            try
            {
                this.RadynTryUpdateModel(CongressType);
                CongressType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Insert(CongressType))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = CongressType.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressType/Index");

            }
            catch (Exception exception)
            {
                ShowMessage(Resources.Common.ErrorInInsert + exception.Message, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return View(CongressType);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(int Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(int Id, FormCollection collection)
        {
            var CongressType = CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(CongressType);
                CongressType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Update(CongressType))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(CongressType);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(int Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            var CongressType = CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressType/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(CongressType);
            }
        }
    }
}