using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;

namespace Radyn.Reservation.Facade.Interface
{
public interface IHallFacade : IBaseFacade<Hall>
{
   
    Hall GetHallWithChairs(Guid hallId);


    bool Insert(Hall hall,  HttpPostedFileBase fileBase);
    bool Update(Hall hall,HttpPostedFileBase fileBase);
    bool HallChairUpdate(List<string> chairs);
    
    IEnumerable<Hall> GetParents();

   
}
}
