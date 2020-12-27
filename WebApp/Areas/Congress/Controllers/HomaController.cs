using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class HomaController : LocalizedController
    {
        public ActionResult Error()
        {
            
            return View();
        }
        public ActionResult GetError()
        {
            
            var keyValuePair = AppExtention.HomaError();
            ViewBag.Error = keyValuePair!=null?keyValuePair.Value.Key:null;
            ViewBag.Photo = keyValuePair != null ? keyValuePair.Value.Value : null;
            return PartialView("PVError");
        }

        [RadynAuthorize]
        public ActionResult SelectCongress()
        {
            var urlReferrer = string.IsNullOrEmpty(Request.QueryString["ReferrerUrl"]) ? (Request.UrlReferrer != null && !Request.UrlReferrer.AbsolutePath.ToLower().Equals("/security/user/login")
                ? Request.UrlReferrer.PathAndQuery
                : string.Empty) : Request.QueryString["ReferrerUrl"];
            var list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetCurrentCongress();
            if (SessionParameters.CurrentCongress != null)
                list = list.Where(x => x.Id != SessionParameters.CurrentCongress.Id).ToList();
            if (!list.Any())
            {
                return Redirect("~/Congress/Homa/Index");
            }
            ViewBag.urlReferrer = urlReferrer;
            ViewBag.CongressList = new SelectList(list, "Id", "CongressTitle");
            return View();
        }

        public ActionResult SelectDropCongress()
        {
            var list = new List<KeyValuePair<string, string>>();
            if (SessionParameters.User != null)
            {
                if (SessionParameters.User.KeyValuePairs == null)
                {
                    var securityUserFacade = CongressComponent.Instance.BaseInfoComponents.SecurityUserFacade;
                    if (securityUserFacade.Any(x =>
                        x.UserId == SessionParameters.User.Id))
                    {
                        var @select = securityUserFacade.Select(x => x.CongressId,
                            x => x.UserId == SessionParameters.User.Id, true);
                        if (select.Any())
                        {
                            list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.SelectKeyValuePair(
                                x => x.Id,
                                x => x.CongressTitle,
                                x => x.Id.In(select) &&
                                     x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 &&
                                     x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 &&
                                     x.Enabled, new OrderByModel<Homa>() {Expression = x => x.Order});
                            SessionParameters.User.KeyValuePairs = list;
                        }
                    }
                    else
                    {
                        list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.SelectKeyValuePair(x => x.Id,
                            x => x.CongressTitle,
                            x =>
                                x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 &&
                                x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 &&
                                x.Enabled, new OrderByModel<Homa>() {Expression = x => x.Order});
                    }
                }
                else
                    list = SessionParameters.User.KeyValuePairs;

            }
            else
            {

                list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.SelectKeyValuePair(x => x.Id,
                   x => x.CongressTitle,
                   x =>
                       x.StartDate.CompareTo(DateTime.Now.ShamsiDate()) <= 0 &&
                       x.EndDate.CompareTo(DateTime.Now.ShamsiDate()) >= 0 &&
                       x.Enabled, new OrderByModel<Homa>() { Expression = x => x.Order });


            }
            ViewBag.CongressList = new SelectList(list, "Key", "Value", SessionParameters.CurrentCongress != null ? SessionParameters.CurrentCongress.Id.ToString() : "");
            return PartialView("PVSelectCongress");

        }

        [HttpPost]
        public ActionResult SelectChangeCongress(string congressId)
        {
            var value = congressId;
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);

                    return Content("false");
                }
                var currentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(value.ToGuid());
                SessionParameters.CurrentCongress = currentCongress;
                ShowMessage(string.Format(Resources.Congress.SuccedEnterToCongress, currentCongress.CongressTitle), Resources.Common.MessaageTitle,
                         messageIcon: MessageIcon.Succeed);
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("true");
            }
        }
        [HttpPost]
        public ActionResult SelectCongress(FormCollection collection)
        {
            try
            {
                var value = collection["SelectedCongressId"];
                var urlReferrer = collection["urlReferrer"];
                if (string.IsNullOrEmpty(value))
                {
                    ViewBag.CongressList = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetCurrentCongress();
                    return View();
                }
                var currentCongress = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(value.ToGuid());
                SessionParameters.CurrentCongress = currentCongress;
                ShowMessage(string.Format(Resources.Congress.SuccedEnterToCongress, currentCongress.CongressTitle), Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Succeed);
                return Redirect((string.IsNullOrEmpty(urlReferrer) ? "~/Security/User/Menu?oid=" + Radyn.Common.Constants.OperationId.CongressOperationId : urlReferrer));
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.CongressList = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetCurrentCongress();
                return View();
            }
        }

         [RadynAuthorize]
        public ActionResult Index()
        {
            Session.Remove("AliasList");
            var list = CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetAll();
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
         [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection)
        {
            var homa = new Homa(){Owner = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode()};
            try
            {

                this.RadynTryUpdateModel(homa);
                this.RadynTryUpdateModel(homa.Owner);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (homa.Owner.EnterpriseNodeTypeId)
                {
                    case 1:
                        homa.Owner.RealEnterpriseNode = new RealEnterpriseNode();
                        this.RadynTryUpdateModel(homa.Owner.RealEnterpriseNode);
                        break;
                    case 2:
                        homa.Owner.LegalEnterpriseNode = new LegalEnterpriseNode();
                        this.RadynTryUpdateModel(homa.Owner.LegalEnterpriseNode);
                        break;
                }
                var list = (List<HomaAlias>)Session["AliasList"];
                homa.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.HomaFacade.Insert(homa, file, list))
                {
                    Session.Remove("AliasList");
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Homa/Edit?Id=" + homa.Id);

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Homa/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);

                return View(homa);
            }
        }


        
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigFromDefault(Guid Id)
        {
           
            try
            {

                if (CongressComponent.Instance.BaseInfoComponents.HomaFacade.ConfigByDefaulToHoma(Id))
                {
                   
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Homa/Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Homa/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Redirect("~/Congress/Homa/Index");
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
         [RadynAuthorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var homa = CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(Id);
            try
            {
                
                this.RadynTryUpdateModel(homa);
                this.RadynTryUpdateModel(homa.Owner);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (homa.Owner.EnterpriseNodeTypeId)
                {
                    case 1:
                        if (homa.Owner.RealEnterpriseNode == null)
                            homa.Owner.RealEnterpriseNode = new RealEnterpriseNode { Id = homa.Owner.Id };
                        this.RadynTryUpdateModel(homa.Owner.RealEnterpriseNode);
                        break;
                    case 2:
                        if (homa.Owner.LegalEnterpriseNode == null)
                            homa.Owner.LegalEnterpriseNode = new LegalEnterpriseNode { Id = homa.Owner.Id };
                        this.RadynTryUpdateModel(homa.Owner.LegalEnterpriseNode);
                        break;
                }
                var list = (List<HomaAlias>)Session["AliasList"];
                homa.CurrentUICultureName = collection["LanguageId"];
                homa.Id = Id;
                if (CongressComponent.Instance.BaseInfoComponents.HomaFacade.Update(homa, file, list))
                {
                    Session.Remove("AliasList");
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Homa/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(homa);
            }
        }

         [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
           
        }
         [RadynAuthorize]
        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {

            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.HomaFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Homa/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Homa/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            var model = Id.HasValue ? CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetLanuageContent(culture, Id) : new Homa() { Enabled = true };
            Session.Remove("AliasList");
            Session["AliasList"] = new List<HomaAlias>();
            if (Id.HasValue) Session["AliasList"] = CongressComponent.Instance.BaseInfoComponents.HomaAliasFacade.GetByCongressId((Guid)Id);
            ViewBag.ENState = Id.HasValue ? "Edit" : "Create";

            ViewBag.CongressTypeList =
                new SelectList(
                    CongressComponent.Instance.BaseInfoComponents.CongressTypeFacade.SelectKeyValuePair(x => x.Id,
                        x => x.Title), "Key", "Value");
            return PartialView("PVModify", model);
        }

        public ActionResult GetDetails(Guid Id)
        {

            return PartialView("PVDetails", CongressComponent.Instance.BaseInfoComponents.HomaFacade.Get(Id));
        }
        public ActionResult GetAlias(Guid? aliasId)
        {
            HomaAlias homaAlias;
            if (aliasId == null || aliasId == Guid.Empty) homaAlias = new HomaAlias();
            else
            {
                var list = (List<HomaAlias>)Session["AliasList"];
                homaAlias = list.FirstOrDefault(@alias => @alias.Id.Equals(aliasId));
            }
            return PartialView("PVAlias", homaAlias);
        }
        [HttpPost]
        public ActionResult GetAlias(FormCollection collection)
        {
            var id = collection["AliasId"].ToGuid();
            var list = (List<HomaAlias>)Session["AliasList"];
            if (list == null) return Content("false");
            var homaAlias = new HomaAlias();
            this.RadynTryUpdateModel(homaAlias);
            if (list.Any(x => x.Url == homaAlias.Url && x.Id != id))
            {
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                          messageIcon: MessageIcon.Error);
                return Content("false");
            }
            var firstOrDefault = list.FirstOrDefault(@alias => @alias.Id.Equals(id));
            if (firstOrDefault != null) this.RadynTryUpdateModel(firstOrDefault);
            else
            {
                homaAlias.Id = Guid.NewGuid();
                list.Add(homaAlias);
            }
            return Content("true");
        }
        public ActionResult DeleteAlias(Guid aliasId)
        {

            var list = (List<HomaAlias>)Session["AliasList"];
            var item = list.FirstOrDefault(ip => ip.Id.Equals(aliasId));
            if (item == null) return Content("false");
            list.Remove(item);
            return Content("true");
        }
        public ActionResult GetAliasList()
        {

            var list = (List<HomaAlias>)Session["AliasList"];
            ViewBag.hiddenEdit = false;
            return PartialView("PVAliasList", list);
        }
        public ActionResult GetAliasListDetails(Guid congressId)
        {

            var list = CongressComponent.Instance.BaseInfoComponents.HomaAliasFacade.GetByCongressId(congressId);
            ViewBag.hiddenEdit = true;
            return PartialView("PVAliasList", list);
        }
    }
}