using System;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Reservation.DA;
using Radyn.Reservation.DataStructure;

namespace Radyn.Reservation.BO
{
    public class ChairTypeBO : BusinessBase<ChairType>
    {
        public override bool Insert(IConnectionHandler connectionHandler, ChairType obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            if (string.IsNullOrEmpty(obj.RefId))
                obj.RefId = obj.Id.ToString();
            return base.Insert(connectionHandler, obj);
        }

        public override bool Delete(IConnectionHandler connectionHandler, params object[] keys)
        {
            var obj = this.Get(connectionHandler, keys);
            var hasInChairType = new ChairBO().HasInChairType(connectionHandler, obj.Id);
            if(hasInChairType)
                throw new Exception("صندلی با  این نوع صندلی وجود دارد و قابل حذف نمیباشد");
            return base.Delete(connectionHandler, keys);
        }


      
    }
}
