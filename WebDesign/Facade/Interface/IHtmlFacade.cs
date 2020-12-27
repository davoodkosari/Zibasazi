using System;
using System.Collections.Generic;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.Framework;
using Radyn.Security.DataStructure;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
    public interface IHtmlFacade : IBaseFacade<Html>
    {
      
        bool Insert(Guid websiteId, HtmlDesgin htmlDesgin);

        List<Partials> GetWebDesignContent(Guid WebId, string culture);
    }
}
