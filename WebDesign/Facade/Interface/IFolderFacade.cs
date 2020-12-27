using System;
using System.Collections.Generic;
using System.Web;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;
using Folder = Radyn.WebDesign.DataStructure.Folder;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IFolderFacade : IBaseFacade<Folder>
{
    IEnumerable<FileManager.DataStructure.Folder> GetParents(Guid websiteId
);

    bool Insert(Guid websiteId, FileManager.DataStructure.Folder folder);
   
    }
}
