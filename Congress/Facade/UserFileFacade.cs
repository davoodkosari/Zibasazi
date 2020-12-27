using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FileManager;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class UserFileFacade : CongressBaseFacade<UserFile>, IUserFileFacade
    {
        internal UserFileFacade()
        {
        }

        internal UserFileFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       




        public bool Insert(Guid congressId, List<HttpPostedFileBase>  file, bool foruser, File fileoptions)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                foreach (var httpPostedFileBase in file)
                {
                    var insert =
                   FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                       .Insert(httpPostedFileBase, fileoptions);
                    if (foruser)
                    {
                        var userFile = new UserFile { CongressId = congressId, FileId = insert };
                        if (!new UserFileBO().Insert(this.ConnectionHandler, userFile))
                            throw new Exception(Resources.Congress.ErrorInSaveFile);
                    }
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

        public bool Update(Guid congressId, HttpPostedFileBase file, Guid fileId, bool foruser, File fileoptions)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var fileTransactionalFacade =
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection);
                if (!fileTransactionalFacade.Update(file, fileId, fileoptions))
                    throw new Exception();
                var userfile = new UserFileBO().Get(this.ConnectionHandler, congressId, fileId);
                if (userfile != null)
                {
                    if (!foruser)
                    {
                        if (!new UserFileBO().Delete(this.ConnectionHandler, congressId, fileId))
                            throw new Exception(Resources.Congress.ErrorInUpdateFile);
                    }
                }
                else
                {
                    if (foruser)
                    {
                        var userFile = new UserFile {CongressId = congressId, FileId = fileId};
                        if (!new UserFileBO().Insert(this.ConnectionHandler, userFile))
                            throw new Exception(Resources.Congress.ErrorInUpdateFile);
                    }


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

        public bool Update(Guid congressId, Guid fileId, bool foruser, File fileoptions)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var insert =
                    FileManagerComponent.Instance.FileTransactionalFacade(this.FileManagerConnection)
                        .Update(fileId, fileoptions);
                if (!insert) return false;
                var userfile = new UserFileBO().Get(this.ConnectionHandler, congressId, fileId);
                if (userfile != null)
                {
                    if (!foruser)
                    {
                        if (!new UserFileBO().Delete(this.ConnectionHandler, congressId, fileId))
                            throw new Exception();
                    }
                }
                else
                {
                    if (foruser)
                    {
                        var userFile = new UserFile {CongressId = congressId, FileId = fileId};
                        if (!new UserFileBO().Insert(this.ConnectionHandler, userFile))
                            throw new Exception();
                    }


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

       
    }
}
