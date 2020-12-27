
using System;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
    public interface IMenuHtmlFacade : IBaseFacade<MenuHtml>
    {
        bool Insert(Guid congressId, Radyn.ContentManager.DataStructure.MenuHtml htmlDesgin);
      

       

        

    }
}
