using System;
using System.Collections.Generic;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Content = Radyn.WebDesign.DataStructure.Content;

namespace Radyn.WebDesign.Facade.Interface
{
    public interface IContentFacade : IBaseFacade<Content>
    {
        IEnumerable<ContentManager.DataStructure.Content> GetByWebSiteId(Guid webSiteId, bool onlyenabled);
        bool Insert(Guid websiteId, ContentManager.DataStructure.Content content, ContentContent contentcontent);
    }
}
