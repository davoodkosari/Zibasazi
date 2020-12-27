using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Radyn.CrossPlatform.DataStructure;
using Radyn.Framework;

namespace Radyn.CrossPlatform.Facade.Interface
{
    public interface IContentCategoriesFacade : IBaseFacade<ContentCategories>
    {
      

        bool Insert(ContentCategories cntCategory, HttpPostedFileBase @base);

        bool Update(ContentCategories cntCategory, HttpPostedFileBase @base);

      

    }
}
