using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.FileManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IUserFileFacade : IBaseFacade<UserFile>
{
    bool Insert(Guid congressId, List<HttpPostedFileBase> file,  bool foruser,File fileoptions);
    bool Update(Guid congressId, HttpPostedFileBase file, Guid fileId, bool foruser, File fileoptions);
    bool Update(Guid congressId, Guid fileId, bool foruser, File fileoptions);
   
}
}
