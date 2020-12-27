using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IBoothFacade : IBaseFacade<Booth>
    {
        IEnumerable<Booth> GetunusedByUserId(Guid userId, Guid congressId);
        IEnumerable<Booth> GerReservableBooth(Guid congressId);
        List<Booth> GetByCongressId(Guid congressId);
    }
}
