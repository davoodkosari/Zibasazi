using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
    public interface IHomaAliasFacade : IBaseFacade<HomaAlias>
    {
        List<HomaAlias> GetByCongressId(Guid congressId);
    }
}
