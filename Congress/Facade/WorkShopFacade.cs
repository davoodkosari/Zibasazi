using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class WorkShopFacade : CongressBaseFacade<WorkShop>, IWorkShopFacade
    {
        internal WorkShopFacade()
        {
        }

        internal WorkShopFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

     

       



        public bool Insert(WorkShop workShop, List<Guid> teacherList, HttpPostedFileBase fileProgram,
            HttpPostedFileBase file)
        {


            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var id = workShop.Id;
                BOUtility.GetGuidForId(ref id);
                workShop.Id = id;
                var fileTransactionalFacade =
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (fileProgram != null)
                    workShop.ProgramAttachId = fileTransactionalFacade.Insert(fileProgram);
                if (file != null)
                    workShop.FileAttachId = fileTransactionalFacade.Insert(file);
                if (!new WorkShopBO().Insert(this.ConnectionHandler, workShop))
                    throw new Exception(Resources.Congress.ErrorInSaveWorkShop);
                if (teacherList.Count > 0)
                {
                    foreach (var guid in teacherList)
                    {
                        var workShopTeacher = new WorkShopTeacher {TeacherId = guid, WorkShopId = workShop.Id};
                        if (!new WorkShopTeacherBO().Insert(this.ConnectionHandler, workShopTeacher))
                            throw new Exception(Resources.Congress.ErrorInSaveWorkShopTeacher);
                    }
                }
                
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(params object[] keys)
        {

            try
            {

                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var fileTransactionalFacade =
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                var obj = new WorkShopBO().Get(this.ConnectionHandler, keys);
                var workShopTeacherBO = new WorkShopTeacherBO();
                var list = workShopTeacherBO.Where(this.ConnectionHandler,
                    teacher => teacher.WorkShopId == obj.Id);
                if (list.Any(guid => !workShopTeacherBO.Delete(this.ConnectionHandler, guid.WorkShopId, guid.TeacherId)))
                {
                    throw new Exception(Resources.Congress.ErrorInDeleteWorkShopTeacher);
                }
                if (!new WorkShopBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception(Resources.Congress.ErrorInDeleteWorkShop);
                if (obj.ProgramAttachId != null)
                {
                    if (!fileTransactionalFacade.Delete(obj.ProgramAttachId))
                        throw new Exception(Resources.Congress.ErrorInDeleteWorkShopprogramFile);
                }
                if (obj.FileAttachId != null)
                {
                    if (!fileTransactionalFacade.Delete(obj.FileAttachId))
                        throw new Exception(Resources.Congress.ErrorInEditWorkShopAttachFile);
                }

                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool Update(WorkShop workShop, List<Guid> teacherList, HttpPostedFileBase fileProgram,
            HttpPostedFileBase file)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                this.CommonConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var list = new WorkShopTeacherBO().Select(this.ConnectionHandler,x=>x.TeacherId,
                    teacher => teacher.WorkShopId == workShop.Id);
                foreach (var guid in list)
                {
                    if (teacherList.All(guid1 => guid1 != guid))
                    {
                        if (!RemoveTeacher(this.ConnectionHandler, workShop.Id, guid))
                            throw new Exception(Resources.Congress.ErrorInDeleteWorkShopTeacher);
                    }
                }

                foreach (var guid in teacherList)
                {
                    if (list.All(guid1 => guid1 != guid))
                    {
                        if (!AddTeacher(this.ConnectionHandler, workShop.Id, guid))
                            throw new Exception(Resources.Congress.ErrorInSaveWorkShopTeacher);
                    }

                }
                var fileTransactionalFacade =
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (fileProgram != null)
                {
                    if (workShop.ProgramAttachId.HasValue)
                    {
                        if (!fileTransactionalFacade.Update(fileProgram, (Guid) workShop.ProgramAttachId))
                            throw new Exception(Resources.Congress.ErrorInEditWorkShopProgramFile);
                    }
                    else workShop.ProgramAttachId = fileTransactionalFacade.Insert(fileProgram);
                }
                if (file != null)
                {
                    if (workShop.FileAttachId.HasValue)
                    {
                        if (!fileTransactionalFacade.Update(file, (Guid) workShop.FileAttachId))
                            throw new Exception(Resources.Congress.ErrorInEditWorkShopAttachFile);
                    }
                    else workShop.FileAttachId = fileTransactionalFacade.Insert(file);
                }
                
                if (!new WorkShopBO().Update(this.ConnectionHandler, workShop))
                    throw new Exception(Resources.Congress.ErrorInEditWorkShop);
                this.ConnectionHandler.CommitTransaction();
                this.CommonConnection.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                this.CommonConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool RemoveTeacher(IConnectionHandler handler, Guid workShopId, Guid teacherId)
        {
            var workShopTeacherBO = new WorkShopTeacherBO();
            var workShopTeacher = workShopTeacherBO.Get(handler, workShopId, teacherId);
            if (workShopTeacher == null) return true;
            if (!workShopTeacherBO.Delete(handler, workShopId, teacherId))
                throw new Exception(Resources.Congress.ErrorInDeleteWorkShopTeacher);
            return true;
        }

        public bool AddTeacher(IConnectionHandler handler, Guid workShopId, Guid teacherId)
        {
            var workShopTeacherBO = new WorkShopTeacherBO();
            var shopTeacher = workShopTeacherBO.Get(handler, workShopId, teacherId);
            if (shopTeacher != null) return true;
            var workShopTeacher = new WorkShopTeacher {TeacherId = teacherId, WorkShopId = workShopId};
            if (!workShopTeacherBO.Insert(handler, workShopTeacher))
                throw new Exception(Resources.Congress.ErrorInSaveWorkShopTeacher);
            return true;
        }
        public List<WorkShop> GetByCongressId(Guid congressId)
        {
            try
            {
                var workShopBo = new WorkShopBO();
                var lst = workShopBo.Where(this.ConnectionHandler, x => x.CongressId == congressId);
                new WorkShopBO().SetCapacity(ConnectionHandler, lst, congressId);
               
                return lst;
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
        public IEnumerable<WorkShop> GetReservableWorkshop(Guid congressId)
        {
            try
            {
                var workShopBo = new WorkShopBO();
                var lst = workShopBo.Where(this.ConnectionHandler, x => x.CongressId == congressId);
                new WorkShopBO().SetCapacity(ConnectionHandler, lst, congressId);
                var list = new List<WorkShop>();
                foreach (var workShop in lst)
                {

                    if (string.IsNullOrEmpty(workShop.Subject)) continue;
                    if (workShop.FreeCapicity > 0)
                        list.Add(workShop);
                }
                return list;
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

    }
}

