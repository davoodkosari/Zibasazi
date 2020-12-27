using Radyn.Congress.DA;
using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class VIPBO : BusinessBase<VIP>
    {
       
        public override bool Insert(IConnectionHandler connectionHandler, VIP obj)
        {
            if (obj.Sort == 0)
                obj.Sort =   (short) (this.GetMaxOrder(connectionHandler)+1);
            return base.Insert(connectionHandler, obj);
        }

        public short GetMaxOrder(IConnectionHandler connectionHandler)
        {
            var vipda = new VIPDA(connectionHandler);
            return vipda.GetMaxOrder();
        }
    }
}
