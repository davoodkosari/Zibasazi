using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Common.Component;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.EnterpriseNode.Tools;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.Utility;

namespace Radyn.WebApp.Areas.EnterpriseNode.Controllers
{
    public class EnterPriseNodeController : LocalizedController
    {


        public ActionResult Index()
        {
            return View(EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.GetAll());
        }
        public ActionResult Details(Guid id)
        {
            return View(EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id));
        }

        public ActionResult Create()
        {
            ViewBag.EnterPriseNodeType =
                new SelectList(EnterpriseNodeComponent.Instance.EnterpriseNodeTypeFacade.GetAll(), "Id", "Title");
            ViewBag.EnterPriseNodeParent = new SelectList(
                EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.GetAll(), "Id", "Title");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var enterprise = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();

            try
            {
                this.RadynTryUpdateModel(enterprise);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (enterprise.EnterpriseNodeTypeId)
                {
                    case 1:
                        enterprise.RealEnterpriseNode = new RealEnterpriseNode();
                        this.RadynTryUpdateModel(enterprise.RealEnterpriseNode);
                        break;
                    case 2:
                        enterprise.LegalEnterpriseNode = new LegalEnterpriseNode();
                        this.RadynTryUpdateModel(enterprise.LegalEnterpriseNode);
                        break;
                }
                if (EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Insert(enterprise, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                              messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }

                return View(enterprise);
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(enterprise);
            }
        }


        public ActionResult Edit(Guid id)
        {
            ViewBag.EnterPriseNodeParent = new SelectList(
              EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.GetAll(), "Id", "Title");
            return View(EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id));
        }
        [HttpPost]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            var enterprise = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);

            try
            {
                this.RadynTryUpdateModel(enterprise);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (enterprise.EnterpriseNodeTypeId)
                {
                    case 1:
                        this.RadynTryUpdateModel(enterprise.RealEnterpriseNode);
                        break;
                    case 2:
                        this.RadynTryUpdateModel(enterprise.LegalEnterpriseNode);
                        break;
                }
                if (EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Update(enterprise, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return View(enterprise);
            }
            catch (Exception exception)
            {
               ShowExceptionMessage(exception);
                return View(enterprise);
            }

        }

        public ActionResult Delete(Guid id)
        {
            return View(EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id));
        }
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            var enterprise = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);

            try
            {
                if (EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Delete(id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                               messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
               ShowExceptionMessage(exception);
                return View(enterprise);
            }
        }

        public ActionResult UploadUserImage(HttpPostedFileBase fileBase)
        {
            HttpPostedFileBase file = Request.Files["upPhotoImage"];
            if (file != null)
            {
                if (file.InputStream != null)
                {

                    Session["Image"] = file;
                }
            }
            return Content("");
        }
        public ActionResult RemoveUploadImage(HttpPostedFileBase fileBase)
        {
            Session.Remove("Image");
            return Content("");
        }
        public ActionResult GenerateLegalInfo(string state, Guid id)
        {
            var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
            LegalEnterpriseNode legalEnterpriseNode = null;
            switch (objectState)
            {
                case Radyn.Common.Definition.ObjectState.Create:
                    legalEnterpriseNode = new LegalEnterpriseNode();
                    if (!string.IsNullOrEmpty(Request.QueryString["title"]))
                        legalEnterpriseNode.Title = Request.QueryString["title"];
                    break;
                case Radyn.Common.Definition.ObjectState.Edit:
                    legalEnterpriseNode =
                        EnterpriseNodeComponent.Instance.LegalEnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.Details:
                    legalEnterpriseNode =
                          EnterpriseNodeComponent.Instance.LegalEnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.Delete:

                    break;
                case Radyn.Common.Definition.ObjectState.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return PartialView("Legalinfo", legalEnterpriseNode);
        }
        public ActionResult GenerateLegalInfoDetails(string state, Guid id)
        {
            var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
            LegalEnterpriseNode legalEnterpriseNode = null;
            switch (objectState)
            {
                case Radyn.Common.Definition.ObjectState.Create:
                    legalEnterpriseNode = new LegalEnterpriseNode();

                    break;
                case Radyn.Common.Definition.ObjectState.Edit:

                    break;
                case Radyn.Common.Definition.ObjectState.Details:
                    legalEnterpriseNode =
                        EnterpriseNodeComponent.Instance.LegalEnterpriseNodeFacade.Get(id);

                    break;
                case Radyn.Common.Definition.ObjectState.Delete:
                    legalEnterpriseNode =
                      EnterpriseNodeComponent.Instance.LegalEnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return PartialView("LegalinfoDetails", legalEnterpriseNode);
        }

        public ActionResult GenerateRealInfo(string state, Guid id)
        {
            var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
            RealEnterpriseNode realEnterpriseNode = null;
            //realEnterpriseNode.GetLanguageContent()

            switch (objectState)
            {
                case Radyn.Common.Definition.ObjectState.Create:
                    realEnterpriseNode = new RealEnterpriseNode();
                    if (!string.IsNullOrEmpty(Request.QueryString["name"]))
                        realEnterpriseNode.FirstName = Request.QueryString["name"];
                    if (!string.IsNullOrEmpty(Request.QueryString["family"]))
                        realEnterpriseNode.LastName = Request.QueryString["family"];
                    realEnterpriseNode.Gender = string.IsNullOrEmpty(Request.QueryString["Gender"]) ||
                                                Request.QueryString["Gender"].ToBool();
                    break;
                case Radyn.Common.Definition.ObjectState.Edit:
                    realEnterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id).RealEnterpriseNode;
                    break;
                case Radyn.Common.Definition.ObjectState.Details:
                    realEnterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id).RealEnterpriseNode;
                    break;
                case Radyn.Common.Definition.ObjectState.Delete:
                    realEnterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id).RealEnterpriseNode;
                    break;
                case Radyn.Common.Definition.ObjectState.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return PartialView("Realinfo", realEnterpriseNode);
        }

        public ActionResult GenerateRealInfoDetails(string state, Guid id)
        {
            var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
            RealEnterpriseNode realEnterpriseNode = null;
            switch (objectState)
            {
                case Radyn.Common.Definition.ObjectState.Create:
                    realEnterpriseNode = new RealEnterpriseNode();
                    break;
                case Radyn.Common.Definition.ObjectState.Edit:

                    break;
                case Radyn.Common.Definition.ObjectState.Details:
                    realEnterpriseNode =
                        EnterpriseNodeComponent.Instance.RealEnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.Delete:
                    realEnterpriseNode =
                       EnterpriseNodeComponent.Instance.RealEnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return PartialView("RealInfoDetails", realEnterpriseNode);
        }

        public ActionResult GenerateEnterpriseNodeModify(string state, Guid id, string type = "n", bool ShowParent = false, bool ShowPicture = true, Guid? ParentId = null)
        {
            var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
            ViewBag.EnterpriseNodeType = new SelectList(EnterpriseNodeComponent.Instance.EnterpriseNodeTypeFacade.GetAll(), "Id", "Title");
            Radyn.EnterpriseNode.DataStructure.EnterpriseNode enterpriseNode = null;
            ViewBag.PrefixTitleList =
                    new SelectList(EnterpriseNodeComponent.Instance.PrefixTitleFacade.GetAll(), "Id", "DescriptionField");
            switch (objectState)
            {
                case Radyn.Common.Definition.ObjectState.Create:
                    enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();
                    if (ParentId != null) enterpriseNode.EnterpriseNodeParentId = ParentId;
                    break;
                case Radyn.Common.Definition.ObjectState.Edit:
                    enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.Details:
                    enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.Delete:
                    enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);
                    break;
                case Radyn.Common.Definition.ObjectState.List:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }

            ViewBag.Type = type;
            ViewBag.ShowParent = ShowParent;
            ViewBag.ShowPicture = ShowPicture;
            ViewBag.State = state;
            if (state == "Delete" || state == "Details") return PartialView("EnterpriseNodeDetails", enterpriseNode);
            return PartialView("EnterpriseNodeModify", enterpriseNode);
        }

        public ActionResult GenerateEnterpriseNodeDetails(string state, Guid id, string type = "n")
        {

            try
            {
                var objectState = state.ToEnum<Radyn.Common.Definition.ObjectState>();
                Radyn.EnterpriseNode.DataStructure.EnterpriseNode enterpriseNode = null;
                switch (objectState)
                {
                    case Radyn.Common.Definition.ObjectState.Create:
                        enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();

                        break;
                    case Radyn.Common.Definition.ObjectState.Edit:

                        break;
                    case Radyn.Common.Definition.ObjectState.Details:
                        enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);
                        break;
                    case Radyn.Common.Definition.ObjectState.Delete:
                        enterpriseNode = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Get(id);
                        break;
                    case Radyn.Common.Definition.ObjectState.List:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("state");
                }
                ViewBag.Type = type;
                ViewBag.State = state;
                return PartialView("EnterpriseNodeDetails", enterpriseNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public ActionResult GenerateTreeLookUp1(int depth)
        {
            return PartialView("TreeLookUp", null);
        }

        public ActionResult Search()
        {
            return this.View();
        }

        [HttpPost]
        public JsonResult SearchEn(FormCollection collection)
        {
            var filter = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();

            switch (collection["state"])
            {
                case "a":
                    filter.LegalEnterpriseNode = new LegalEnterpriseNode { Title = collection["title"] };
                    filter.RealEnterpriseNode = new RealEnterpriseNode() { FirstName = collection["title"], LastName = collection["title"] };
                    ViewBag.Title = collection["title"];
                    break;
                case "s":
                    var type = collection["type"];
                    if (!string.IsNullOrEmpty(type))
                    {
                        switch (type)
                        {
                            case "r":
                                filter.EnterpriseNodeTypeId = 1;
                                filter.RealEnterpriseNode = new RealEnterpriseNode
                                {
                                    FirstName = collection["fname"].Trim(),
                                    LastName = collection["lname"].Trim(),
                                    NationalCode = collection["nationalId"].Trim()
                                };
                                break;
                            case "l":
                                filter.EnterpriseNodeTypeId = 2;
                                filter.LegalEnterpriseNode = new LegalEnterpriseNode
                                {
                                    Title = collection["legalTitle"].Trim(),
                                    NationalId = collection["nationalCode"].Trim(),
                                    RegisterNo = collection["registerNo"].Trim()
                                };
                                break;
                        }
                    }
                    break;
            }
            var result = new List<object>
                             {
                                 new
                                     {
                                         id = "",
                                         title = "عنوان",
                                         type = "نوع شخص",
                                         nationalCode = "كد/شناسه ملي",
                                         registerNo = "شماره شناسنامه/ شماره ثبت"
                                     }
                             };

            try
            {
                var enterpriseNodes = EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Search(filter).ToList();
                if (enterpriseNodes.Any())
                {
                    foreach (var node in enterpriseNodes)
                    {
                        result.Add(new
                        {
                            id = node.Id.ToString(),
                            title = node.Title(),
                            type = node.EnterpriseNodeTypeId == 1 ? "حقيقي" : "حقوقي",
                            nationalCode =
                                       node.LegalEnterpriseNode != null
                                           ? node.LegalEnterpriseNode.NationalId
                                           : node.RealEnterpriseNode.NationalCode,
                            registerNo =
                                       node.LegalEnterpriseNode != null
                                           ? node.LegalEnterpriseNode.RegisterNo
                                           : node.RealEnterpriseNode.IDNumber

                        });
                    }
                }
                else
                {
                    var messageText = string.Format("{0}<br/><br/><a href='{2}'><font color='green'>{1}</font></a>",
                                                    Resources.Common.No_results_found,
                                                    Resources.EnterPriseNode.DeclareNewEnterprisenode,
                                                    string.Format(
                                                        "/EnterpriseNode/EnterpriseNode/CreateEnterPriseNode?Etype={0}&name={1}&family={2}&nid={3}",
                                                        (collection["type"]),
                                                        (filter.RealEnterpriseNode != null
                                                             ? filter.RealEnterpriseNode.FirstName
                                                             : filter.LegalEnterpriseNode.Title),
                                                        (filter.RealEnterpriseNode != null
                                                             ? filter.RealEnterpriseNode.LastName
                                                             : filter.LegalEnterpriseNode.RegisterNo),
                                                        (filter.RealEnterpriseNode != null
                                                             ? filter.RealEnterpriseNode.NationalCode
                                                             : filter.LegalEnterpriseNode.NationalId)));
                    ShowMessage(messageText, Resources.Common.Search, new[] { Resources.Common.Cancel, "" });
                }
            }
            catch
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Lookup()
        {
            return PartialView("Lookup");
        }

        public ActionResult CreateEnterPriseNode()
        {
            var enterpriseNode =
                new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
                {
                    RealEnterpriseNode = new RealEnterpriseNode(),
                    LegalEnterpriseNode = new LegalEnterpriseNode()
                };
            var name = Request.QueryString["name"];
            if (!string.IsNullOrEmpty(name))
                if (enterpriseNode.RealEnterpriseNode != null)
                    enterpriseNode.RealEnterpriseNode.FirstName = name;
                else enterpriseNode.LegalEnterpriseNode.Title = name;
            var family = Request.QueryString["family"];
            if (!string.IsNullOrEmpty(family))
                if (enterpriseNode.RealEnterpriseNode != null)
                    enterpriseNode.RealEnterpriseNode.LastName = family;
                else enterpriseNode.LegalEnterpriseNode.RegisterNo = name;
            var nationalId = Request.QueryString["nid"];
            if (!string.IsNullOrEmpty(nationalId))
            {
                if (enterpriseNode.RealEnterpriseNode != null)
                    enterpriseNode.RealEnterpriseNode.NationalCode = nationalId;
                else enterpriseNode.LegalEnterpriseNode.NationalId = nationalId;
            }
            var etype = Request.QueryString["Etype"];
            if (!string.IsNullOrEmpty(etype))
                enterpriseNode.EnterpriseNodeTypeId = (etype == "R" ? (byte)Radyn.EnterpriseNode.Tools.Enums.EnterpriseNodeType.RealEnterPriseNode : (byte)Radyn.EnterpriseNode.Tools.Enums.EnterpriseNodeType.LegalEnterPriseNode);
            return View(enterpriseNode);
        }
        [HttpPost]
        public ActionResult CreateEnterPriseNode(FormCollection collection)
        {
            try
            {
                var enterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode();
                this.RadynTryUpdateModel(enterpriseNode, collection);
                var type = collection["type"];
                if (string.IsNullOrEmpty(collection["EnterpriseNodeTypeId"]))
                {
                    if (string.IsNullOrEmpty(type)) return null;
                    enterpriseNode.EnterpriseNodeTypeId = type == "r" ? 1 : 2;
                }
                else enterpriseNode.EnterpriseNodeTypeId = collection["EnterpriseNodeTypeId"].ToInt();
                if (enterpriseNode.EnterpriseNodeTypeId == 1)
                {
                    enterpriseNode.RealEnterpriseNode = new RealEnterpriseNode();
                    this.RadynTryUpdateModel(enterpriseNode.RealEnterpriseNode, collection);
                }
                else if (enterpriseNode.EnterpriseNodeTypeId == 2)
                {
                    enterpriseNode.LegalEnterpriseNode = new LegalEnterpriseNode();
                    this.RadynTryUpdateModel(enterpriseNode.LegalEnterpriseNode, collection);
                }
                var file = Session["Image"];
                if (EnterpriseNodeComponent.Instance.EnterpriseNodeFacade.Insert(enterpriseNode, (HttpPostedFileBase)file))
                {
                    object obj = new { id = enterpriseNode.Id, title = enterpriseNode.Title() };
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                return null;

            }
        }
    }
}
