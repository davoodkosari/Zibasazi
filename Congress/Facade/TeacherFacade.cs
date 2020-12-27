using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.EnterpriseNode;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class TeacherFacade : CongressBaseFacade<Teacher>, ITeacherFacade
    {
        internal TeacherFacade()
        {
        }

        internal TeacherFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


     

        public bool Insert(Teacher teacher, 
            HttpPostedFileBase fileResume, HttpPostedFileBase file)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var id = teacher.Id;
                BOUtility.GetGuidForId(ref id);
                teacher.Id = id;
                teacher.EnterpriseNode.Id = teacher.Id;
                if (fileResume != null)
                    teacher.ResumeAttachId =
                        FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Insert(fileResume);

                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Insert(teacher.EnterpriseNode, file))
                    return false;
                
                if (!new TeacherBO().Insert(this.ConnectionHandler, teacher))
                    throw new Exception(Resources.Congress.ErrorInSaveTeacher);
                
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Teacher teacher, 
            HttpPostedFileBase fileResume, HttpPostedFileBase file)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (fileResume != null)
                {
                    if (teacher.ResumeAttachId.HasValue)
                    {
                        if (
                            !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Update(fileResume,
                                    (Guid)teacher.ResumeAttachId))
                            throw new Exception(Resources.Congress.ErrorInSaveResume);
                    }
                    else
                        teacher.ResumeAttachId =
                            FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                                .Insert(fileResume);
                }
               
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Update(teacher.EnterpriseNode, file))
                    return false;
                if (!new TeacherBO().Update(this.ConnectionHandler, teacher))
                    throw new Exception(Resources.Congress.ErrorInEditTeacher);
               
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public List<KeyValuePair<Teacher, bool>> GetWorkShopTeacherModel(Guid congressId, Guid? workShopId)
        {
            try
            {
                var list = new TeacherBO().Where(this.ConnectionHandler, x => x.CongressId == congressId);
                var getAllForArticle = new List<KeyValuePair<Teacher, bool>>();
                var guids=new List<Guid>();
                if (workShopId.HasValue)
                    guids = new WorkShopTeacherBO().Select(ConnectionHandler, x => x.TeacherId,
                        x => x.WorkShopId == workShopId);
                foreach (var teacher in list)
                {
                    var added = guids.Any(x=>x.Equals(teacher.Id));
                    getAllForArticle.Add(new KeyValuePair<Teacher, bool>(teacher, added));
                }
                return getAllForArticle;
            }
            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.EnterpriseNodeConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new TeacherBO().Get(this.ConnectionHandler, keys);
                var workShopTeacherBO = new WorkShopTeacherBO();
                var byFilter = workShopTeacherBO.Where(ConnectionHandler, teacher => teacher.TeacherId == obj.Id);
                if (
                    byFilter.Any(
                        workShopTeacher =>
                            !workShopTeacherBO.Delete(this.ConnectionHandler, workShopTeacher.WorkShopId,
                                workShopTeacher.TeacherId)))
                    throw new Exception(Resources.Congress.ErrorInDeleteWorkShopTeacher);
                if (!new TeacherBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInSaveTeacher);
                if (
                    !EnterpriseNodeComponent.Instance.EnterpriseNodeTransactionalFacade(this.EnterpriseNodeConnection)
                        .Delete(obj.Id))
                    return false;
                if (obj.ResumeAttachId.HasValue)
                {
                    if (
                        !FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                            .Delete((Guid)obj.ResumeAttachId))
                        throw new Exception(Resources.Congress.ErrorInDeleteResume);
                }
                this.ConnectionHandler.CommitTransaction();
                this.EnterpriseNodeConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.EnterpriseNodeConnection.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}
