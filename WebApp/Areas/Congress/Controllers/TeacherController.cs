using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radyn.Congress;
using Radyn.Congress.DataStructure;
using Radyn.EnterpriseNode;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.Web.Mvc.UI.Message;
using Radyn.WebApp.AppCode.Security;
using Radyn.WebApp.Areas.Congress.Security.Filter;
using Radyn.WebApp.Areas.Common.Tools;
using Radyn.WebApp.Areas.EnterpriseNode.ExtentionTools;

namespace Radyn.WebApp.Areas.Congress.Controllers
{
    public class TeacherController : CongressBaseController
    {
        [RadynAuthorize]
        public ActionResult Index()
        {

            var list = CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Where(
                    x => x.CongressId == this.Homa.Id);
            return View(list);
        }

        [RadynAuthorize]
        public ActionResult Details(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Get(Id));
        }

        [RadynAuthorize]
        public ActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var teacher = new Teacher(){EnterpriseNode = new Radyn.EnterpriseNode.DataStructure.EnterpriseNode { RealEnterpriseNode = new RealEnterpriseNode() }};
            teacher.EnterpriseNode.EnterpriseNodeTitle();
            try
            {
                this.RadynTryUpdateModel(teacher);
              this.RadynTryUpdateModel(teacher.EnterpriseNode);
                this.RadynTryUpdateModel(teacher.EnterpriseNode.RealEnterpriseNode);
               
                HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                HttpPostedFileBase fileResume = null;
                if (Session["ImageResume"] != null)
                {
                    fileResume = (HttpPostedFileBase)Session["ImageResume"];
                    Session.Remove("ImageResume");
                }
                teacher.CongressId = this.Homa.Id;
                if (CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Insert(teacher, fileResume, file))
                {
                    ShowMessage(Resources.Common.InsertSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection);
                }
                ShowMessage(Resources.Common.ErrorInInsert, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Teacher/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);

                return View(teacher);
            }
        }

        [RadynAuthorize]
        public ActionResult Edit(Guid Id)
        {

            return View(CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Get(Id));
        }

        [HttpPost]
        public ActionResult Edit(Guid Id, FormCollection collection)
        {
            var teacher = CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Get(Id);
            try
            {
                this.RadynTryUpdateModel(teacher);
                this.RadynTryUpdateModel(teacher.EnterpriseNode);
                this.RadynTryUpdateModel(teacher.EnterpriseNode.RealEnterpriseNode);
               HttpPostedFileBase file = null;
                if (Session["Image"] != null)
                {
                    file = (HttpPostedFileBase)Session["Image"];
                    Session.Remove("Image");
                }
                HttpPostedFileBase fileResume = null;
                if (Session["ImageResume"] != null)
                {
                    fileResume = (HttpPostedFileBase)Session["ImageResume"];
                    Session.Remove("ImageResume");
                }
              if (CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Update(teacher, fileResume, file))
                {
                    ShowMessage(Resources.Common.UpdateSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return this.SubmitRedirect(collection);
                }
                ShowMessage(Resources.Common.ErrorInEdit, Resources.Common.MessaageTitle, messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Teacher/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(teacher);
            }
        }

        public ActionResult GetTeachers(Guid? workShopId, bool modifymode = true)
        {
            ViewBag.ModifyMode = modifymode;
            return PartialView("PartialViewTeachers", CongressComponent.Instance.BaseInfoComponents.TeacherFacade.GetWorkShopTeacherModel(this.Homa.Id, workShopId).ToList());
        }
        [RadynAuthorize]
        public ActionResult Delete(Guid Id)
        {
            return View(CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Get(Id));
        }
        [HttpPost]
        public ActionResult Delete(Guid Id, FormCollection collection)
        {
            var teacher = CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Get(Id);
            try
            {
                if (CongressComponent.Instance.BaseInfoComponents.TeacherFacade.Delete(Id))
                {
                    ShowMessage(Resources.Common.DeleteSuccessMessage, Resources.Common.MessaageTitle,
                                messageIcon: MessageIcon.Succeed);
                    return Redirect("~/Congress/Teacher/Index");
                }
                ShowMessage(Resources.Common.ErrorInDelete, Resources.Common.MessaageTitle,
                            messageIcon: MessageIcon.Error);
                return Redirect("~/Congress/Teacher/Index");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                return View(teacher);
            }
        }
       
    }
}