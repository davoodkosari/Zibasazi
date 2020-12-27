using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Framework;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class PivotController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.PivotFacade.OrderBy(x => x.Order,
                    x => x.CongressId == this.Homa.Id);
            return View(list);
        }
        public ActionResult GetPivot(Guid? categoryId)
        {
            var cat = new List<Pivot>();
            var obj = new Object();
            var result = new List<object>();
            var pivotFacade = CongressComponent.Instance.BaseInfoComponents.PivotFacade;
            var predicateBuilder = new PredicateBuilder<Pivot>();
            predicateBuilder.And(x => x.CongressId == this.Homa.Id);
            if (categoryId.HasValue && categoryId != Guid.Empty)
                predicateBuilder.And(x => x.PivotCategoryId == categoryId);

            cat = pivotFacade.OrderBy(x => x.Order, predicateBuilder.GetExpression());
            foreach (var item in cat)
            {
                obj = new
                {
                    Id = item.Id,
                    Title = item.Title
                };
                result.Add(obj);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.PivotFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x => x.CongressId == this.Homa.Id, new OrderByModel<PivotCategory>() { Expression = x => x.Order }), "Key", "Value");
            return View(new Pivot());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var pivot = new Pivot();
            try
            {
                this.RadynTryUpdateModel(pivot);
                pivot.CongressId = this.Homa.Id;
                pivot.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.PivotFacade.Insert(pivot))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = pivot.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Pivot/Index");
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
            ViewBag.Category = new SelectList(CongressComponent.Instance.BaseInfoComponents.PivotCategoryFacade.SelectKeyValuePair(x => x.Id, x => x.Title, x => x.CongressId == this.Homa.Id, new OrderByModel<PivotCategory>() { Expression = x => x.Order }), "Key", "Value");
            return View(CongressComponent.Instance.BaseInfoComponents.PivotFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var pivot = CongressComponent.Instance.BaseInfoComponents.PivotFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(pivot);
                pivot.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.PivotFacade.Update(pivot))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Pivot/Index");
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
            return View(CongressComponent.Instance.BaseInfoComponents.PivotFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var pivot = CongressComponent.Instance.BaseInfoComponents.PivotFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.PivotFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Pivot/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Pivot/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(pivot);
            }
        }
        [RadynAuthorize]
        public ActionResult LookUPRefereePivot(Guid pivotId)
        {
            ViewBag.pivot = pivotId;
            return View();
        }
        [HttpPost]
        public ActionResult LookUPRefereePivot(FormCollection collection)
        {

            try
            {

                var list = new List<Guid>();
                var Id = collection["pivotId"].ToGuid();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedRefree"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        list.Add(variable.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereePivotFacade.UpdatePivotReferee(Id, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        public ActionResult GetReferess(Guid pivotId)
        {

            var list = CongressComponent.Instance.BaseInfoComponents.RefereePivotFacade.GetBypivotId(pivotId);
            return PartialView("PVRefereePivot", list);
        }
        public ActionResult SearchRefereepivot(string txt, Guid pivotId)
        {
            var list = CongressComponent.Instance.BaseInfoComponents.RefereePivotFacade.Search(txt, pivotId, this.Homa.Id);
            return PartialView("PVRefereePivot", list);
        }
    }
}