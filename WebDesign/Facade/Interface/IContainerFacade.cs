using System;
using System.Collections.Generic;
using Radyn.Framework;
using Container = Radyn.WebDesign.DataStructure.Container;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IContainerFacade : IBaseFacade<Container>
{
    IEnumerable<ContentManager.DataStructure.Container> GetByWebSiteId(Guid websiteId);
    bool Insert(Guid websiteId, ContentManager.DataStructure.Container container);
    
}
}
