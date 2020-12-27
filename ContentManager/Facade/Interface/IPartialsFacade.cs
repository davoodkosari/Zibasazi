using System;
using System.Collections;
using System.Collections.Generic;
using Radyn.ContentManager.DataStructure;
using Radyn.ContentManager.Definition;
using Radyn.ContentManager.Tools;
using Radyn.Framework;
using Radyn.Security.DataStructure;

namespace Radyn.ContentManager.Facade.Interface
{
    public interface IPartialsFacade : IBaseFacade<Partials>
    {
        bool DeletePartialWithUrl(string url);
       
        List<Partials> GetContentPartials(IEnumerable<int> idlist, string culture);
        List<Partials> GetOperationPartials(Guid OperationId);
        List<Partials> GetContentPartials(string culture);
      
    }
}
