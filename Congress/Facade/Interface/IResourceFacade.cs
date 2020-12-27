using System;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IResourceFacade : IBaseFacade<Resource>
{
    bool Insert(Resource resource, HttpPostedFileBase resourceFile);
    bool Update(Resource resource, HttpPostedFileBase resourceFile);
    string GetCongressResourceHtml(Guid websiteId, string culture, Enums.UseLayout layout);
}
}
