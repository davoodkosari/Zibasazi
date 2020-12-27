using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.FileManager;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressFoldersFacade : CongressBaseFacade<CongressFolders>, ICongressFoldersFacade
    {
        internal CongressFoldersFacade()
        {
        }

        internal CongressFoldersFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

     
        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var obj = new CongressFoldersBO().Get(this.ConnectionHandler, keys);
                if (!new CongressFoldersBO().Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف پوشه فایل همایش وجود دارد");
                if (
                    !FileManagerComponent.Instance.FolderTransactionalFacade(this.FileManagerConnection)
                        .Delete(obj.FolderId))
                    throw new Exception(Resources.Congress.ErrorInDeleteCongressFolder);
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

        public bool Insert(Guid congressId, Folder folder)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                folder.IsExternal = true;
                if (!FileManagerComponent.Instance.FolderTransactionalFacade(this.FileManagerConnection).Insert(folder))
                    throw new Exception("خطایی در ذخیره پوشه فایل وجود دارد");
                var congressContent = new CongressFolders {CongressId = congressId, FolderId = folder.Id};
                if (!new CongressFoldersBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressFolder);
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

      
       
        public Folder GetFirstParent()
        {
            try
            {

                return FileManagerComponent.Instance.FolderFacade.GetFirstParent(true);

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

        public Dictionary<File, bool> GetFolderFiles(Guid congressId, Guid folderId)
        {
            try
            {
                var getFolderFiles = new Dictionary<File, bool>();
                var list = FileManagerComponent.Instance.FileFacade.Where(x => x.FolderId == folderId);
                var userFileBo = new UserFileBO();
                var @select = userFileBo.Select(ConnectionHandler, x => x.FileId, x => x.CongressId == congressId);
                foreach (var file in list)
                {
                    var foruser = @select.Any(x=>x.Equals(file.Id));
                    getFolderFiles.Add(file, foruser);
                }
                return getFolderFiles;
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
