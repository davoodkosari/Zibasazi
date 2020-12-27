using System;
using System.Web;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IResourceFacade : IBaseFacade<Resource>
{
    bool Insert(Resource resource, HttpPostedFileBase resourceFile);
    bool Update(Resource resource, HttpPostedFileBase resourceFile);
    string GetWebSiteResourceHtml(Guid websiteId, string culture);
}
}
