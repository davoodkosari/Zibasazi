using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Utility;
using Radyn.Web.Html;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class BoothController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.BoothFacade.GetByCongressId(this.Homa.Id);
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
            var articlePaymentType = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(Id);
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
            ViewBag.BoothCatgory = new SelectList(CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.SelectKeyValuePair(x=>x.Id,x=>x.Title,x => x.CongressId == this.Homa.Id), "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new Booth());
            var languageContent = CongressComponent.Instance.BaseInfoComponents.BoothFacade.GetLanuageContent(culture,Id);
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
            var booth = new Booth();
            try
            {
                this.RadynTryUpdateModel(booth);
                booth.CongressId = this.Homa.Id;
                booth.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.BoothFacade.Insert(booth))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = booth.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Booth/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(booth);
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
            var booth = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(booth);
                booth.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.BoothFacade.Update(booth))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Booth/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(booth);
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
            var booth = CongressComponent.Instance.BaseInfoComponents.BoothFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.BoothFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Booth/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Booth/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View(booth);
            }
        }
        public ActionResult GetBoothOfficer(Guid? Id)
        {
            if (Session["BoothOfficers"] == null) Session["BoothOfficers"] = new List<BoothOfficer>();
            BoothOfficer articleAuthors;
            if (Id == null || Id == Guid.Empty)
                articleAuthors = new BoothOfficer()
                {
                    EnterpriseNode =
                        new Radyn.EnterpriseNode.DataStructure.EnterpriseNode()
                        {
                            RealEnterpriseNode = new RealEnterpriseNode()
                        }
                        ,
                    Id = Guid.NewGuid()
                };
            else
            {
                var list = (List<BoothOfficer>)Session["BoothOfficers"];
                articleAuthors = list.FirstOrDefault(organization => organization.Id.Equals(Id));
            }
            ViewBag.PrefixTitleList =
                new SelectList(EnterpriseNodeComponent.Instance.PrefixTitleFacade.GetAll(), "Id", "DescriptionField", articleAuthors != null ? articleAuthors.EnterpriseNode.PrefixTitleId : null);
            return PartialView("PVBoothOfficer", articleAuthors);
        }
        [HttpPost]
        public ActionResult GetBoothOfficer(FormCollection collection)
        {
            var isnew = false;
            var id = collection["BoothOfficerId"].ToGuid();
            var list = (List<BoothOfficer>)Session["BoothOfficers"];
            if (list == null) return Content("false");
            var messageStack = new List<string>();
            var firstOrDefault = list.FirstOrDefault(organizationIp => organizationIp.Id.Equals(id));
            if (firstOrDefault == null)
            {
                isnew = true;
                firstOrDefault = new BoothOfficer
                {
                    EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode
                    {
                        RealEnterpriseNode = new RealEnterpriseNode()
                    },

                };
            }

            this.RadynTryUpdateModel(firstOrDefault);
            this.RadynTryUpdateModel(firstOrDefault.EnterpriseNode);
            this.RadynTryUpdateModel(firstOrDefault.EnterpriseNode.RealEnterpriseNode);
            if (Session["BoothOfficerImage"] != null)
            {
                firstOrDefault.AttachFile = (HttpPostedFileBase)Session["BoothOfficerImage"];
                Session.Remove("BoothOfficerImage");
            }
            if (string.IsNullOrEmpty(firstOrDefault.EnterpriseNode.RealEnterpriseNode.FirstName))
                messageStack.Add(Resources.Congress.Please_Enter_YourName);
            if (string.IsNullOrEmpty(firstOrDefault.EnterpriseNode.RealEnterpriseNode.LastName))
                messageStack.Add(Resources.Congress.Please_Enter_YourLastName);
            if (string.IsNullOrEmpty(firstOrDefault.EnterpriseNode.Cellphone))
                messageStack.Add(Resources.Congress.Please_Enter_YourMobile);
            if (firstOrDefault.EnterpriseNode.RealEnterpriseNode.Gender == null)
                messageStack.Add(Resources.Congress.Please_Enter_YourGender);
            if (string.IsNullOrEmpty(firstOrDefault.EnterpriseNode.Email))
                messageStack.Add(Resources.Congress.PleaseEnterYourEmail);
            else
            {
                
                if (!Utility.Utils.IsEmail(firstOrDefault.EnterpriseNode.Email))
                    messageStack.Add(Resources.Congress.UnValid_Enter_Email);
            }
            var messageBody = messageStack.Aggregate("", (current, item) => current + Tag.Li(item));
            if (messageBody != "")
            {
                ShowMessage(messageBody, Resources.Common.Attantion, messageIcon: MessageIcon.Warning);
                return Content("false");
            }
            if (!isnew) return Content("true");
            firstOrDefault.Id = Guid.NewGuid();
            firstOrDefault.Order = list.Count == 0 ? 1 : list.Max(x => x.Order) + 1;
            list.Add(firstOrDefault);
            return Content("true");
        }
        public ActionResult DeleteBoothOfficer(Guid Id)
        {
            var list = (List<BoothOfficer>)Session["BoothOfficers"];
            var item = list.FirstOrDefault(ip => ip.Id.Equals(Id));
            if (item == null) return Content("false");
            list.Remove(item);
            return Content("true");
        }
        public ActionResult GetBoothOfficerList()
        {
            ViewBag.hiddenEdit = false;
            ViewBag.AllowPrintCard = false;
            var list = (List<BoothOfficer>)Session["BoothOfficers"] ?? new List<BoothOfficer>();
            return PartialView("PVBoothOfficerList", list.OrderBy(authors => authors.Order));
        }
        public ActionResult GetBoothOfficerListDetailList(Guid userId, Guid boothId)
        {
            ViewBag.hiddenEdit = true;
            var userBooth = CongressComponent.Instance.BaseInfoComponents.UserBoothFacade.Get(userId, boothId);
            ViewBag.AllowPrintCard = userBooth != null && (userBooth.Status == (byte)Enums.RezervState.PayConfirm || userBooth.Status == (byte)Enums.RezervState.Finalconfirm);
            ViewBag.userId = userId;
            ViewBag.boothId = boothId;
            var list = CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.OrderBy(x=>x.Order,x =>
            x.UserId == userId&&
                x.BoothId == boothId
            );
            return PartialView("PVBoothOfficerList", list);
        }
        public ActionResult GetGuestBoothOfficerDetailList(Guid guestId)
        {
            ViewBag.hiddenEdit = true;
            var list = CongressComponent.Instance.BaseInfoComponents.BoothOfficerFacade.OrderBy(x=>x.Order,x =>
            
                x.UserId == guestId
            );
            return PartialView("PVBoothOfficerList", list);
        }
        public ActionResult RemoveArticleFile(HttpPostedFileBase fileBase, string filename)
        {
            Session.Remove(filename);
            return Content("");
        }
      
    }
}