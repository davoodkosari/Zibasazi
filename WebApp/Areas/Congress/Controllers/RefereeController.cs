using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class RefereeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.GetAllrefreeWithCartable(this.Homa.Id);
            return View(list);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {


            if (!string.IsNullOrEmpty(collection["txtSearch"]))
            {
                var lst =
                    CongressComponent.Instance.BaseInfoComponents.RefereeFacade.SearchRefree(collection["txtSearch"], this.Homa.Id);
                return View(lst);
            }
            var list =
               CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Where(
                   refree => refree.CongressId == this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
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
        public ActionResult Create(FormCollection collection)
        {
            var referee = new Referee();
            try
            {
                this.RadynTryUpdateModel(referee);
                referee.CongressId = this.Homa.Id;
                if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Insert(referee))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = referee.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Referee/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);

                return View(referee);
            }
        }

        [HttpPost]
        public ActionResult CreateReferee(FormCollection collection)
        {
            var referee = new Referee(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode { RealEnterpriseNode = new RealEnterpriseNode() }};
            try
            {
               
                this.RadynTryUpdateModel(referee);
                this.RadynTryUpdateModel(referee.EnterpriseNode);
                this.RadynTryUpdateModel(referee.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                referee.CongressId = this.Homa.Id;
                var list = new List<Guid>();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedPivot"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        if (string.IsNullOrEmpty(variable)) continue;
                        list.Add(variable.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Insert(referee, file, list))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='/Congress/Referee/Index'; " }, messageIcon: MessageIcon.Succeed);
                    return Json(new { Result = true, Url = this.CallBackRedirect(collection, new { Id = referee.Id }) }, JsonRequestBehavior.AllowGet);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Json(new { Result = false, Url = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetModify(Guid? Id)
        {
            var model = Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Get(Id) : new Referee() { Enabled = true };
            //var model= Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.RefereeFacade.GetModify(Id,HomaId): new Referee() { Enabled = true };
            ViewBag.ENState = Id.HasValue ? "Edit" : "Create";
            return PartialView("PVModify", model);
        }

        public ActionResult GetDetails(Guid Id)
        {
            try
            {
                var detail = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Get(Id);
                return PartialView("PVDetails", detail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [RadynAuthorize]
        public ActionResult Edit(Guid? Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var referee = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(referee);
                this.RadynTryUpdateModel(referee.EnterpriseNode);
                this.RadynTryUpdateModel(referee.EnterpriseNode.RealEnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                var messageStack = new List<string>();
                //                if (referee.SendInform && string.IsNullOrEmpty(referee.Password))
                //                    messageStack.Add(Resources.Congress.Please_Enter_Password);
                if (!string.IsNullOrEmpty(referee.Password))
                {
                    if (referee.Password != collection["RepeatPassword"])
                        messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
                }
                var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
                if (messageBody != "")
                {
                    ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                    return View(referee);
                }
                var list = new List<Guid>();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedPivot"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        if (string.IsNullOrEmpty(variable)) continue;
                        list.Add(variable.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Update(referee, file, list))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Referee/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(referee);
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

            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Referee/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Referee/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
        [RadynAuthorize]
        [HttpPost]
        public ActionResult Validate(FormCollection collection)
        {

            var messageStack = new List<string>();
            var referee = new Referee(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode { RealEnterpriseNode = new RealEnterpriseNode() }};
            this.RadynTryUpdateModel(referee);
            this.RadynTryUpdateModel(referee.EnterpriseNode);
            this.RadynTryUpdateModel(referee.EnterpriseNode.RealEnterpriseNode);
            if (string.IsNullOrEmpty(referee.Password))
                messageStack.Add(Resources.Congress.Please_Enter_Password);
            if (string.IsNullOrEmpty(collection["RepeatPassword"]))
                messageStack.Add(Resources.Congress.Please_Enter_Password_Repeat);
            else if (!string.IsNullOrEmpty(referee.Password) && !string.IsNullOrEmpty(collection["RepeatPassword"]))
                if (referee.Password != collection["RepeatPassword"])
                    messageStack.Add(Resources.Congress.Password_and_Repeat_Not_Equal);
            if (string.IsNullOrEmpty(referee.Username))
                messageStack.Add(Resources.Congress.PleaseInsertUserName);
           var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
            if (messageBody != "")
            {
                ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }
            return Content("true");

        }
        [RadynAuthorize]
        public ActionResult LookUPRefereePivot(Guid refreeId)
        {
            ViewBag.refreeId = refreeId;
            return View();
        }

        [RadynAuthorize]
        public ActionResult LookUpArticleAssign(Guid refreeId)
        {
            ViewBag.refreeId = refreeId;
            var list =
                CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.Where(
                    i => i.RefereeId == refreeId);
            return View(list);
        }

        [HttpPost]
        public ActionResult LookUpArticleAssign(FormCollection collection)
        {
            try
            {
                var refereeId = collection["RefreeId"];
                var lst = collection.AllKeys.Where(s => s.StartsWith("Articlec")).Select(key => key.Substring(8, key.Length - 8)).Select(id => id.ToGuid()).ToList();
                if (lst.Count == 0)
                {
                    ShowMessage(string.Format(Resources.Congress.ThisArticleHasNotBeenSelectedForRemoval,this.Homa.Configuration.ArticleTitle), Resources.Common.MessaageTitle,
                      messageIcon: MessageIcon.Warning);
                    return Content("false");
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereeCartableFacade.DeleteFromRefreeCartable(lst, refereeId.ToGuid()))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Content("true");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Content("false");
            }
            catch (Exception exception)
            {

                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult LookUPRefereePivot(FormCollection collection)
        {

            try
            {

                var list = new List<Guid>();
                var Id = collection["refreeId"].ToGuid();
                var enumerable = collection.AllKeys.FirstOrDefault(s => s.Equals("SelectedPivot"));
                if (enumerable != null)
                {
                    var strings = collection[enumerable].Split(',');
                    foreach (var variable in strings)
                    {
                        if (string.IsNullOrEmpty(variable)) continue;
                        list.Add(variable.ToGuid());
                    }
                }
                if (CongressComponent.Instance.BaseInfoComponents.RefereePivotFacade.Update(Id, list))
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
        public ActionResult Refereepivots(Guid? refreeId, bool enable = false)
        {
            try
            {
                var list = CongressComponent.Instance.BaseInfoComponents.RefereePivotFacade.GetByRefereeId(this.Homa.Id, refreeId);
                ViewBag.enable = enable;
                return PartialView("PVRefereePivot", list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public ActionResult GetImportData()
        {

            HttpPostedFileBase file = null;
            if (Session["Image"] != null)
            {
                file = (HttpPostedFileBase)Session["Image"];
                Session.Remove("Image");
            }
            var importFromExcel = CongressComponent.Instance.BaseInfoComponents.RefereeFacade.ImportFromExcel(file, this.Homa.Id);
            Session["RefereeList"] = importFromExcel;
            return PartialView("PVImportFromExcel", importFromExcel);
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromExcel(FormCollection collection)
        {
            try
            {

                var referee = new List<Referee>();
                if (Session["RefereeList"] == null) RedirectToAction("Index");
                var list = (Dictionary<Referee, List<string>>)Session["RefereeList"];
                var firstOrDefault = collection.AllKeys.FirstOrDefault(s => s.Equals("Checkselect"));
                if (string.IsNullOrEmpty(collection[firstOrDefault]))
                {
                    ShowMessage(Resources.Common.No_results_found, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                    return RedirectToAction("Index");

                }
                var strings = collection[firstOrDefault].Split(',');
                foreach (var vale in strings)
                {
                    if (string.IsNullOrEmpty(vale)) continue;
                    var orDefault = list.Keys.FirstOrDefault(x => x.Id == vale.ToGuid());
                    if (orDefault != null)
                        referee.Add(orDefault);
                }

                if (CongressComponent.Instance.BaseInfoComponents.RefereeFacade.InsertList(referee))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    Session.Remove("RefereeList");
                    return RedirectToAction("Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View();
            }
        }


        public ActionResult RemoveReferee(Guid Id)
        {
            if (Session["RefereeList"] == null) return Content("false");
            var list = (Dictionary<Referee, List<string>>)Session["RefereeList"];
            var any = list.FirstOrDefault(x => x.Key.Id == Id);
            if (any.Key == null) return Content("false");
            list.Remove(any.Key);
            return Content("true");
        }
    }
}