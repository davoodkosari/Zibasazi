using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressFoldersFacade : IBaseFacade<CongressFolders>
{
    bool Insert(Guid congressId, Folder folder);
    
  
    Folder GetFirstParent();
    Dictionary<File, bool> GetFolderFiles(Guid congressId,Guid folderId);
}
}
