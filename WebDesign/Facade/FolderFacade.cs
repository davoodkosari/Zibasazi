using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Radyn.FileManager;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.FileManager.DataStructure;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.Facade.Interface;
using Folder = Radyn.WebDesign.DataStructure.Folder;

namespace Radyn.WebDesign.Facade
{
    internal sealed class FolderFacade : WebDesignBaseFacade<Folder>, IFolderFacade
    {
        internal FolderFacade() { }

        internal FolderFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }
    

        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var folderBo = new FolderBO();
                var obj = folderBo.Get(this.ConnectionHandler, keys);
                if (!folderBo.Delete(this.ConnectionHandler, keys))
                    throw new Exception("خطایی در حذف پوشه فایل  وجود دارد");
                if (!FileManagerComponent.Instance.FolderTransactionalFacade(this.FileManagerConnection).Delete(obj.FolderId))
                    throw new Exception("خطایی در حذف پوشه فایل  وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }

        public IEnumerable<FileManager.DataStructure.Folder> GetParents(Guid websiteId)
        {
            try
            {

                var list = FileManagerComponent.Instance.FolderFacade.GetParents(true);
                var listfolder = new List<FileManager.DataStructure.Folder>();
                var congressFoldersBo = new FolderBO();
                foreach (var folder in list)
                {
                    var congressFolders = congressFoldersBo.Get(this.ConnectionHandler, websiteId, folder.Id);
                    if (congressFolders == null) continue;
                    listfolder.Add(folder);
                }
                return listfolder.OrderBy(folder => folder.Title);
            }
            catch (KnownException knownException)
            {

                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {

                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Insert(Guid websiteId, FileManager.DataStructure.Folder folder)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                folder.IsExternal = true;
                if (!FileManagerComponent.Instance.FolderTransactionalFacade(this.FileManagerConnection).Insert(folder))
                    throw new Exception("خطایی در ذخیره پوشه فایل وجود دارد");
                var congressContent = new Folder { WebId = websiteId, FolderId = folder.Id };
                if (!new FolderBO().Insert(this.ConnectionHandler, congressContent))
                    throw new Exception("خطایی در ذخیره پوشه فایل  وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }

     
    }
}
