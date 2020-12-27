using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IHotelFacade : IBaseFacade<Hotel>
    {
       
        IEnumerable<Hotel> GetReservableHotel(Guid congressId);

        List<Hotel> GetByCongressId(Guid congressId);
    }
}
