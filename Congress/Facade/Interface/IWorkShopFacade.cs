using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IWorkShopFacade : IBaseFacade<WorkShop>
    {
        bool Insert(WorkShop workShop, List<Guid> teacherList, HttpPostedFileBase fileProgram, HttpPostedFileBase file);
        bool Update(WorkShop workShop, List<Guid> teacherList, HttpPostedFileBase fileProgram, HttpPostedFileBase file);
     
      
        IEnumerable<WorkShop> GetReservableWorkshop(Guid congressId);
        List<WorkShop> GetByCongressId(Guid congressId);
    }
}
