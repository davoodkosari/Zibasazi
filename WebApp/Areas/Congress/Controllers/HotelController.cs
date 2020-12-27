using System;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Common.Component;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Utility;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class HotelController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.HotelFacade.GetByCongressId(this.Homa.Id);
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
            var hotel = new Hotel();
            try
            {
                this.RadynTryUpdateModel(hotel);
                hotel.CongressId = this.Homa.Id;
                hotel.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.HotelFacade.Insert(hotel))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = hotel.Id });
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
               return Redirect("~/Congress/Hotel/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Lanuages = new SelectList(CommonComponent.Instance.LanguageFacade.Where(x=>x.Enabled), "Id", "DisplayName");
                return View(hotel);
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
            var hotel = CongressComponent.Instance.BaseInfoComponents.HotelFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(hotel);
                hotel.CurrentUICultureName = collection["LanguageId"];
                if (CongressComponent.Instance.BaseInfoComponents.HotelFacade.Update(hotel))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
               return Redirect("~/Congress/Hotel/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Id = Id;
                return View();
            }
        }
      
        public ActionResult GetDetail(Guid Id)
        {
            var hotel = CongressComponent.Instance.BaseInfoComponents.HotelFacade.Get(Id);
            if (hotel == null) return Content("false");
            return PartialView("PVDetails", hotel);
        }
        public ActionResult GetModify(Guid? Id, string culture)
        {
            if (string.IsNullOrEmpty(culture)) culture = SessionParameters.Culture;
            ViewBag.Currency =
                new SelectList(
                    EnumUtils.ConvertEnumToIEnumerableInLocalization<Radyn.Common.Definition.Enums.CurrencyType>(),
                    "Key", "Value");
            if (!Id.HasValue) return PartialView("PVModify", new Hotel());
            return PartialView("PVModify",CongressComponent.Instance.BaseInfoComponents.HotelFacade.GetLanuageContent(culture,Id));
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult DeleteHotel(Guid Id)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.HotelFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle, new[] { Resources.Common.Ok, " window.location='"+Radyn.Web.Mvc.UI.Application.CurrentApplicationPath+"/Congress/Hotel/Index'; " }, messageIcon: MessageIcon.Succeed);
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
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.HotelFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                   return Redirect("~/Congress/Hotel/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
               return Redirect("~/Congress/Hotel/Index");
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