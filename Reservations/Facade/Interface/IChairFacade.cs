using System;
using System.Collections.Generic;
using Radyn.Framework;
using Radyn.Reservation.DataStructure;
using Radyn.Reservation.Definition;

namespace Radyn.Reservation.Facade.Interface
{
public interface IChairFacade : IBaseFacade<Chair>
{
    bool ChangeStatusAndSetOwner(Guid chairId, Enums.ReservStatus reservStatus = Enums.ReservStatus.Reserved, Guid? ownerId = null);
    IEnumerable<Chair> GetListChairByIdList(List<Guid> list);
}
}
