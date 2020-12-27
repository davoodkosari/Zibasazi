using System;
using System.Collections.Generic;
using Radyn.Congress.DataStructure;
using Radyn.Framework;

namespace Radyn.Congress.Facade.Interface
{
public interface IRefereePivotFacade : IBaseFacade<RefereePivot>
{
    Dictionary<Pivot, bool> GetByRefereeId(Guid congressId, Guid? refreeId);
    bool Insert(Guid refreeId, List<Guid> pivots);
    bool Update(Guid refreeId, List<Guid> pivots);
    bool UpdatePivotReferee(Guid pivotId, List<Guid> refereeId);
    Dictionary<Referee, bool> Search(string txt, Guid pivotId, Guid id);
    Dictionary<Referee, bool> GetBypivotId(Guid pivotId);
}
}
