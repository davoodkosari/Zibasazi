using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.Tools;
using Radyn.Payment;
using Radyn.Payment.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Congress.Tools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressDiscountTypeController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.GetByCongressId(this.Homa.Id);
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
            ViewBag.Id = Guid.NewGuid();
            return View();
        }
        public ActionResult GenerateConfigDiscountSection()
        {
            ViewBag.Dicounttype = CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.GetByCongressId(this.Homa.Id).Where(x=>x.Enabled);
            ViewBag.EnumsSource = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.PaymentSection>().Select(
                   keyValuePair =>
                       new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.PaymentSection>(),
                           keyValuePair.Value)).ToList();
            return PartialView("PartialViewConfigDiscounTypeSection", PaymentComponenets.Instance.DiscountTypeSectionFacade.GetByModelName(AppExtention.CongressMoudelName));
        }
        public ActionResult GenerateDiscountSection(Guid Id,bool Readonly=false)
        {
            ViewBag.Dicounttype = Id;
            ViewBag.EnumsSource = EnumUtils.ConvertEnumToIEnumerableInLocalization<Enums.PaymentSection>().Select(
                    keyValuePair =>
                        new KeyValuePair<byte, string>((byte)keyValuePair.Key.ToEnum<Enums.PaymentSection>(),
                            keyValuePair.Value)).ToList(); 
            ViewBag.Readonly = Readonly;
            return PartialView("PartialViewDiscounTypeSection", PaymentComponenets.Instance.DiscountTypeSectionFacade.GetByModelName(AppExtention.CongressMoudelName));
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var discountType = new DiscountType();
            try
            {
                this.RadynTryUpdateModel(discountType);

                var sectiontypes = Payment.Tools.AppExtentions.FillDiscountTypeSection(collection, AppExtention.CongressMoudelName);
                var discountTypeAutoCodes = Payment.Tools.AppExtentions.FillDiscountAutoCode(discountType, collection);
                discountType.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.Insert(this.Homa.Id, discountType, sectiontypes, discountTypeAutoCodes))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = discountType.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressDiscountType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id=Guid.NewGuid();
                return View();
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
            var discountType = PaymentComponenets.Instance.DiscountTypeFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(discountType);
                var sectiontypes = Payment.Tools.AppExtentions.FillDiscountTypeSection(collection, AppExtention.CongressMoudelName);
                var discountTypeAutoCodes = Payment.Tools.AppExtentions.FillDiscountAutoCode(discountType,collection);
                discountType.CurrentUICultureName = collection["LanguageId"];
                if (PaymentComponenets.Instance.DiscountTypeFacade.Update(discountType, AppExtention.CongressMoudelName, sectiontypes, discountTypeAutoCodes))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressDiscountType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
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
                if (CongressComponent.Instance.BaseInfoComponents.CongressDiscountTypeFacade.Delete(this.Homa.Id,Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressDiscountType/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressDiscountType/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
       
    }
}
