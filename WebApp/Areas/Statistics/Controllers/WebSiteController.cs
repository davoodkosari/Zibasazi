using System;
using System.Web;
using System.Web.Mvc;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Statistics;
using Radyn.Statistics.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Base;
using Radyn.WebApp.AppCode.Security;


namespace Radyn.WebApp.Areas.Statistics.Controllers
{
    public class WebSiteController : LocalizedController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {
            var list = StatisticComponents.Instance.WebSiteFacade.GetAll();
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(StatisticComponents.Instance.WebSiteFacade.Get(Id));
        }

        [RadynAuthorize]
        
        public ActionResult Create()
        {

            return View(new WebSite());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var webSite = new WebSite(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode() };
            try
            {
                this.RadynTryUpdateModel(webSite);
                this.RadynTryUpdateModel(webSite.EnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (webSite.EnterpriseNode.EnterpriseNodeTypeId)
                {
                    case 1:
                        webSite.EnterpriseNode.RealEnterpriseNode = new RealEnterpriseNode();
                        this.RadynTryUpdateModel(webSite.EnterpriseNode.RealEnterpriseNode);
                        break;
                    case 2:
                        webSite.EnterpriseNode.LegalEnterpriseNode = new LegalEnterpriseNode();
                        this.RadynTryUpdateModel(webSite.EnterpriseNode.LegalEnterpriseNode);
                        break;
                }
                webSite.Title = collection["WebSiteTitle"];
                if (StatisticComponents.Instance.WebSiteFacade.Insert(webSite, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(webSite);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {
            return View(StatisticComponents.Instance.WebSiteFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var webSite = StatisticComponents.Instance.WebSiteFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(webSite);
                this.RadynTryUpdateModel(webSite.EnterpriseNode);
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                switch (webSite.EnterpriseNode.EnterpriseNodeTypeId)
                {
                    case 1:
                        this.RadynTryUpdateModel(webSite.EnterpriseNode.RealEnterpriseNode);
                        break;
                    case 2:
                        this.RadynTryUpdateModel(webSite.EnterpriseNode.LegalEnterpriseNode);
                        break;
                }

                webSite.Title = collection["WebSiteTitle"];
                if (StatisticComponents.Instance.WebSiteFacade.Update(webSite, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return RedirectToAction("Index");
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(webSite);
            }
        }

        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(StatisticComponents.Instance.WebSiteFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var webSite = StatisticComponents.Instance.WebSiteFacade.Get(Id);
            try
            {
                if (StatisticComponents.Instance.WebSiteFacade.Delete(Id))
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
                return View(webSite);
            }
        }
    }
}
