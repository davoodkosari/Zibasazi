using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class PivotCategoryController : CongressBaseController
    {
        // GET: Congress/PivotCategory
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.OrderBy(x=>x.Order,x=>x.CongressId==this.Homa.Id);
            return View(list);
        }


        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View(new PivotCategory());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var pivot = new PivotCategory();
            try
            {
                RadynTryUpdateModel(pivot, collection);
                pivot.CongressId = this.Homa.Id;
                pivot.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Insert(pivot))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = pivot.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/PivotCategory/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(pivot);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var pivot = CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(pivot,collection);
                pivot.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Update(pivot))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/PivotCategory/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(pivot);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var pivot = CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/PivotCategory/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/PivotCategory/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(pivot);
            }
        }

    }
}