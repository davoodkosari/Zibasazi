using System;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class BoothCatgoryController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Where(
                    x => x.CongressId == this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
           
            return View(new BoothCatgory());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var boothCatgory = new BoothCatgory();
            try
            {
                this.RadynTryUpdateModel(boothCatgory);
                boothCatgory.CongressId = this.Homa.Id;
                if (CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Insert(boothCatgory))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/BoothCatgory/Index");

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/BoothCatgory/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Homa = new SelectList(CongressComponent.Instance.BaseInfoComponents.HomaFacade.GetAll(), "Id", "Title");
                return View(boothCatgory);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
           
            return View(CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var boothCatgory = CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(boothCatgory);
                if (CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Update(boothCatgory))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/BoothCatgory/Index");

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/BoothCatgory/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(boothCatgory);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var boothCatgory = CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.BoothCatgoryFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/BoothCatgory/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/BoothCatgory/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(boothCatgory);
            }
        }
    }
}