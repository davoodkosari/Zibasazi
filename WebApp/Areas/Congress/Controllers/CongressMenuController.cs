﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Radyn.Common.Component;
using Radyn.Common.DataStructure;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressMenuController : CongressBaseController
    {

        [RadynAuthorize]
        public ActionResult Index()
        {
            var list =
                CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Select(x => x.Menu,
                    menu => menu.CongressId == this.Homa.Id);
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {

            var predicateBuilder = new PredicateBuilder<CongressMenu>();
            predicateBuilder.And(x => x.CongressId == this.Homa.Id);


            if (!string.IsNullOrEmpty(collection["title"]))
            {
                predicateBuilder.And(x => x.Menu.Text.Contains(collection["title"]));
            }
            var list = CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Select(x => x.Menu, predicateBuilder.GetExpression());
            return View(list);
        }


        [RadynAuthorize]
        public ActionResult Details(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }
        [RadynAuthorize]
        public ActionResult Create()
        {

            return View();
        }



        public ActionResult SearchCongressMenu(string title)
        {


            var list =
                CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Select(x => x.Menu,
                    menu => menu.CongressId == this.Homa.Id);
            if (!string.IsNullOrEmpty(title))
            {
                var lst = list.Where(item => item.Text.Contains(title)).ToList();
                return PartialView("PVSearchIndex", lst);
            }

            return PartialView("PVSearchIndex", list);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var menu = new Menu();

            try
            {
                this.RadynTryUpdateModel(menu);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                menu.CurrentUICultureName = collection["LanguageId"]; ;
                if (CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Insert(this.Homa.Id, menu, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = menu.Id });


                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressMenu/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.CongressId = this.Homa.Id;
                return View(menu);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid id, string culture)
        {
            ViewBag.Id = id;
            ViewBag.Culture = culture;
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            var menu = ContentManagerComponent.Instance.MenuFacade.Get(id);

            try
            {
                this.RadynTryUpdateModel(menu);

                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }


                menu.CurrentUICultureName = collection["LanguageId"]; ;
                if (ContentManagerComponent.Instance.MenuFacade.Update(menu, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = menu.Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressMenu/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(menu);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            var menu = ContentManagerComponent.Instance.MenuFacade.Get(id);

            try
            {
                if (ContentManagerComponent.Instance.MenuFacade.Any(x => x.ParentId == menu.Id))
                {
                    ShowMessage(Resources.ContentManager.This_Item_Not_able_to_Delete_Becase_Have_ChildNode,
                                Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Error);
                    return Redirect("~/Congress/CongressMenu/Index");

                }
                if (CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Delete(this.Homa.Id, id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressMenu/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressMenu/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = id;
                return View(menu);
            }
        }
        [RadynAuthorize]
        public ActionResult ModifyMenu(FormCollection formCollection)
        {
            try
            {
                var id = formCollection["menuid"];
                var state = formCollection["menuState"];
                var parentorOwn = string.IsNullOrEmpty(id) ? null : ContentManagerComponent.Instance.MenuFacade.Get(Guid.Parse(id));
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (state)
                {

                    case "Create":
                        {

                            var menu = new Menu();
                            this.RadynTryUpdateModel(menu, formCollection);
                            menu.CurrentUICultureName = formCollection["LanguageId"];
                            if (CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Insert(this.Homa.Id, menu, file))
                            {

                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                                        messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                    case "Edit":
                        {
                            if (parentorOwn == null)
                            {
                                ShowMessage(Resources.ContentManager.Not_Exist_To_Edit, Resources.Common.MessaageTitle,
                                            messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            this.RadynTryUpdateModel(parentorOwn, formCollection);
                            parentorOwn.CurrentUICultureName = formCollection["LanguageId"];
                            if (ContentManagerComponent.Instance.MenuFacade.Update(parentorOwn, file))
                            {

                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                    case "Delete":
                        {
                            if (parentorOwn == null)
                            {
                                ShowMessage(Resources.ContentManager.Not_Exist_To_delete, Resources.Common.MessaageTitle,
                                            messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            if (ContentManagerComponent.Instance.MenuFacade.Any(menu => menu.ParentId == parentorOwn.Id))
                            {
                                ShowMessage(Resources.ContentManager.This_Item_Not_able_to_Delete_Becase_Have_ChildNode,
                                            Resources.Common.MessaageTitle,
                                            messageIcon: MessageIcon.Error);
                                return Content("false");
                            }
                            if (CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.Delete(this.Homa.Id, parentorOwn.Id))
                            {

                                return Content("true");
                            }
                            ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                                        messageIcon: MessageIcon.Error);
                            return Content("false");
                        }
                }
                return Content("true");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return Content("false");
            }
        }
        [RadynAuthorize]
        public ActionResult PartialViewModifyMenu(string state, Guid id)
        {
            ViewBag.state = state;
            switch (state)
            {
                case "Create":
                    var menu1 = ContentManagerComponent.Instance.MenuFacade.Get(id);
                    ViewBag.ParentId = menu1 != null ? menu1.Id : (Guid?)null;
                    return PartialView("PVModifyMenu");
                case "Edit":
                    ViewBag.Id = id;
                    return PartialView("PVModifyMenu");
                case "Delete":
                    ViewBag.Id = id;
                    return PartialView("PVDetailsMenu");
            }
            return Content("");
        }

        [RadynAuthorize]
        public ActionResult LookUpMenu(string culture)
        {
            ViewBag.Culture = culture;
            ViewBag.treeId = DateTime.Now.Ticks.ToString();
            return View();
        }

        public ActionResult GetMenuTree(string culture)
        {
            culture = string.IsNullOrEmpty(culture) ? SessionParameters.Culture : culture;
            var model = CongressComponent.Instance.BaseInfoComponents.CongressMenuFacade.MenuTree(this.Homa.Id, culture);
            ViewBag.treeId = DateTime.Now.Ticks.ToString();
            return PartialView("PartialViewMenuTree", model);
        }

    }
}
