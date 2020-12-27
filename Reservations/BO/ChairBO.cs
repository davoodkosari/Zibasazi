using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.DA;
using Radyn.Reservation.DataStructure;

namespace Radyn.Reservation.BO
{
    internal class ChairBO : BusinessBase<Chair>
    {
        public override bool Insert(IConnectionHandler connectionHandler, Chair obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }

        public Chair GetByPosition(IConnectionHandler connectionHandler, Guid hallId, int column, int row)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.GetByPosition(hallId, column, row);
        }

        public bool UpdateChairs(IConnectionHandler connectionHandler, List<string> chairs)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.UpdateChairs(chairs)>= 0;
        }

        public bool InsertChairs(IConnectionHandler connectionHandler, List<Chair> chairs)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.InsertChairs(chairs) >= 0;
        }
        public List<Chair> GetHallChairs(IConnectionHandler connectionHandler, Guid hallId)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.GetHallChairs(hallId);
        }

        public bool AllowDelete(IConnectionHandler connectionHandler, Guid id)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.AllowDelete(id)==0;
        }

        public bool HasInChairType(IConnectionHandler connectionHandler, Guid chairtypeId)
        {
            var chairDa = new ChairDA(connectionHandler);
            return chairDa.HasInChairType(chairtypeId) > 0;
        }

        public IEnumerable<Chair> GetListChairByIdList(IConnectionHandler connectionHandler, List<Guid> list)
        {
            var chairDa = new ChairDA(connectionHandler);
            if (list==null||!list.Any()) return null;
            return chairDa.GetListChairByIdList(list);
        }
    }
}
