using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongessGalleryFacade : IBaseFacade<CongessGallery>
    {
        bool Insert(Guid congressId, Gallery.DataStructure.Gallery gallery, HttpPostedFileBase fileBase);
        bool Insert(Guid congressId, Gallery.DataStructure.Gallery gallery, HttpPostedFileBase fileBase, List<HttpPostedFileBase> httpPostedFileBases);
        List<KeyValuePair<string, string>> GetParents(Guid homaId);
    }
}
