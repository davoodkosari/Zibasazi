using System;
using Radyn.Congress.DataStructure;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ICongressMenuHtmlFacade : IBaseFacade<CongressMenuHtml>
    {
        bool Insert(Guid congressId, MenuHtml htmlDesgin);
        bool Update(Guid congressId, MenuHtml htmlDesgin);

       

        

    }
}
