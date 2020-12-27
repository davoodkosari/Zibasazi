using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Framework;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IGalleryFacade : IBaseFacade<DataStructure.Gallery>
{
    IEnumerable<Gallery.DataStructure.Gallery> GetParents(Guid websiteId);
    bool Insert(Guid websiteId,  Gallery.DataStructure.Gallery gallery, HttpPostedFileBase image);
}
}
