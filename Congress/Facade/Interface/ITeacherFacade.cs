using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface ITeacherFacade : IBaseFacade<Teacher>
    {
       

        bool Insert(Teacher teacher,  HttpPostedFileBase fileResume, HttpPostedFileBase file );
        bool Update(Teacher teacher,  HttpPostedFileBase fileResume, HttpPostedFileBase file);
        List<KeyValuePair<Teacher,bool>> GetWorkShopTeacherModel(Guid congressId, Guid? workShopId);
    }
}
