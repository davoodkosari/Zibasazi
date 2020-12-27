using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.ContentManager.BO
{
    internal class ContainerBO : BusinessBase<Container>
    {
        public override bool Insert(IConnectionHandler connectionHandler, Container obj)
        {
            var id = obj.Id;
            BOUtility.GetGuidForId(ref id);
            obj.Id = id;
            return base.Insert(connectionHandler, obj);
        }
    }
}
