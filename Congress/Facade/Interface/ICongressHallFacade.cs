using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Reservation.DataStructure;

namespace Radyn.Congress.Facade.Interface
{
public interface ICongressHallFacade : IBaseFacade<CongressHall>
{
    bool Insert(Guid congressId, Hall hall,  HttpPostedFileBase fileBase, List<Guid> userregistertype);
    bool Update(Guid congressId, Hall hall,  HttpPostedFileBase fileBase, List<Guid> userregistertype);
    IEnumerable<Hall> GetByCongressId(Guid congressId,bool onlyEnalbe=false);
    IEnumerable<Hall> GetByParents(Guid congressId, Guid hallId, bool onlyEnalbe=false);
    IEnumerable<Hall> GetParents(Guid congressId, bool onlyEnalbe = false);
    Dictionary<UserRegisterPaymentType, bool> GetChairTypes(Guid congressId, Guid? hallId);
    IEnumerable<User> GetReservedList(Guid congressId, Guid hallId);
}
}
