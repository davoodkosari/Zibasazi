using Radyn.Congress.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class BoothCatgoryBO : BusinessBase<BoothCatgory>
    {
        public override bool Insert(IConnectionHandler connectionHandler, BoothCatgory obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }
    }
}
