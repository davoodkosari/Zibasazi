using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressHtmlFacade : IBaseFacade<CongressHtml>
    {
        bool Insert(Guid congressId, HtmlDesgin htmlDesgin);
        bool Update(Guid congressId, HtmlDesgin htmlDesgin);



        List<Partials> GetWebDesignContent(Guid websiteId, string culture);

     

    }
}
