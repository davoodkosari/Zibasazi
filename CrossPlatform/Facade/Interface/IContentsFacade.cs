using System;
using System.Collections.Generic;
using System.Web;
using Radyn.CrossPlatform.DataStructure;
using Radyn.Framework;

namespace Radyn.CrossPlatform.Facade.Interface
{
    public interface IContentsFacade : IBaseFacade<Contents>
    {
      
        IEnumerable<Contents> GetListByCategory(Guid congressId, Guid categoryId);

      

        bool Insert(Contents obj, Guid? userId, HttpPostedFileBase @base);

        bool Update(Contents obj, Guid? userId, HttpPostedFileBase @base);

        bool Delete(Guid Id, Guid? userId);
    }
}
