using System;
using System.Web.Mvc;
using Radyn.Common;
using Radyn.Congress;
using Radyn.Payment;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class CongressAccountController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = CongressComponent.Instance.BaseInfoComponents.CongressAccountFacade.Select(x=>x.Account,x=>x.CongressId==this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Int16 Id)
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
            var account = new Radyn.Payment.DataStructure.Account();
            try
            {
                this.RadynTryUpdateModel(account);
                if (CongressComponent.Instance.BaseInfoComponents.CongressAccountFacade.Insert(this.Homa.Id,account))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = account.Id });

                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressAccount/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                ViewBag.Bank = new SelectList(CommonComponent.Instance.BankFacade.GetAll(), "Id", "Title");
                return View(account);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Int16 Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Int16 Id, FormCollection collection)
        {
            var account = PaymentComponenets.Instance.AccountFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(account);
                if (PaymentComponenets.Instance.AccountFacade.Update(account))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection, new { Id = Id });

                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressAccount/Index");

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(account);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Int16 Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Int16 Id, FormCollection collection)
        {
          
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.CongressAccountFacade.Delete(this.Homa.Id, Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/CongressAccount/Index");

                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/CongressAccount/Index");

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